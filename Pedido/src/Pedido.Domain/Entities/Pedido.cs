using Pedido.Domain.Custumer.Entities;
using Pedido.Domain.Exceptions;
using Pedido.Domain.Shared.Entities;
using Pedido.Domain.ValueObjects;

namespace Pedido.Domain.Entities
{
    public class Pedido : Entity
    {
        public Cliente? Cliente { get; private set; }
        public Guid? ClienteId { get; private set; }

        public IReadOnlyCollection<PedidoItem> Items => _items.AsReadOnly();
        private readonly List<PedidoItem> _items;

        public PedidoStatus Status { get; private set; }
        public decimal TotalPrice { get; private set; }

        public string? QrCode { get; private set; }
        public string? PreferenceId { get; private set; }

        private Pedido() : base()
        {
            _items = new List<PedidoItem>();
            Status = PedidoStatus.Pending;
            TotalPrice = 0;
        }

        private Pedido(Guid? clienteId, List<PedidoItem> items) : base()
        {
            ClienteId = clienteId;
            _items = items ?? new List<PedidoItem>();
            Status = PedidoStatus.Pending;
            CalculateTotalPrice();
        }

        public static Pedido Create(Guid? customerId, List<PedidoItem> items)
        {
            if (customerId.HasValue && customerId.Value == Guid.Empty)
                throw new PedidoDomainException("Quando informado, o ID do cliente deve ser um GUID válido");

            if (items == null || !items.Any())
                throw new PedidoDomainException("O pedido deve ter pelo menos um item");

            return new Pedido(customerId, items);
        }

        public void AddItem(PedidoItem item)
        {
            if (Status != PedidoStatus.Pending)
                throw new PedidoDomainException("Não é possível adicionar itens a um pedido que não está pendente");

            if (item == null)
                throw new PedidoDomainException("O item não pode ser nulo");

            _items.Add(item);
            CalculateTotalPrice();
            SetUpdatedAt();
        }

        public bool RemoveItem(Guid produtoId)
        {
            if (Status != PedidoStatus.Pending)
                throw new PedidoDomainException("Não é possível remover itens a um pedido que não está pendente");

            var item = _items.FirstOrDefault(i => i.ProdutoId == produtoId);
            if (item == null)
                return false;

            var removed = _items.Remove(item);
            if (removed)
            {
                CalculateTotalPrice();
                SetUpdatedAt();
            }
            return removed;
        }

        public void UpdateStatus(PedidoStatus status)
        {
            if (!IsValidStatusTransition(Status, status))
                throw new PedidoDomainException($"A transição do status {Status} para {status} não é permitida");

            Status = status;
            SetUpdatedAt();
        }

        public void SetQrCode(string qrCode)
        {
            if (string.IsNullOrWhiteSpace(qrCode))
                throw new PedidoDomainException("O QR Code não pode ser vazio");

            QrCode = qrCode;
            SetUpdatedAt();
        }

        public void SetPreferenceId(string preferenceId)
        {
            if (string.IsNullOrWhiteSpace(preferenceId))
                throw new PedidoDomainException("O ID da preferência não pode ser vazio");

            PreferenceId = preferenceId;
            SetUpdatedAt();
        }

        private bool IsValidStatusTransition(PedidoStatus currentStatus, PedidoStatus newStatus)
        {
            return (currentStatus, newStatus) switch
            {
                (PedidoStatus.Pending, PedidoStatus.Processing) => true,
                (PedidoStatus.Processing, PedidoStatus.Ready) => true,
                (PedidoStatus.Ready, PedidoStatus.Completed) => true,
                (PedidoStatus.Pending, PedidoStatus.Cancelled) => true,
                (PedidoStatus.Processing, PedidoStatus.Cancelled) => true,

                (PedidoStatus.Pending, PedidoStatus.AwaitingPayment) => true,
                (PedidoStatus.AwaitingPayment, PedidoStatus.Paid) => true,
                (PedidoStatus.AwaitingPayment, PedidoStatus.Cancelled) => true,
                (PedidoStatus.Paid, PedidoStatus.Processing) => true,

                (PedidoStatus.Pending, PedidoStatus.Paid) => true,
                (PedidoStatus.Processing, PedidoStatus.Pending) => true,

                var (current, next) when current == next => true,
                _ => false
            };
        }

        private void CalculateTotalPrice()
        {
            TotalPrice = _items.Sum(item => item.SubTotal);
        }

        public void SetStatusDireto(PedidoStatus status)
        {
            Status = status;
        }
    }
}
