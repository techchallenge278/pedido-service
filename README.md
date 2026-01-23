# Microsserviço Pedido

## Descrição
O microsserviço **Pedido** é responsável por gerenciar todo o fluxo de pedidos da aplicação, incluindo:
- Registro de novos pedidos
- Listagem de pedidos existentes
- Visão do pedido para o cliente
- Integração com os microsserviços de Pagamento e Produção

## Observações
CI/CD configurado via GitHub Actions  
Branch main protegida, PR obrigatório  
Testes unitários com cobertura ≥80%  
<img width="1183" height="189" alt="image" src="https://github.com/user-attachments/assets/99ab573e-6655-4e0f-bb31-e09f638d349a" />

## Tecnologias
- .NET 8
- C#
- SQL Server
- GitHub Actions para CI/CD
- Docker para containerização

## Funcionalidades
- Criar um novo pedido
- Consultar pedidos por cliente ou status
- Atualizar status do pedido
- Validar dados de entrada com testes unitários (cobertura ≥80%)

## Como executar
### Local
1. Clone o repositório
```bash
git clone <URL_DO_REPOSITORIO>
cd pedido
```

2. Build e restore
```bash
dotnet restore
dotnet build
```

3. Rodar testes
```bash
dotnet restore
dotnet build
```

4. Executar container Docker
```bash
docker build -t pedido-service:latest .
docker run -d -p 5001:5000 --name pedido-service pedido-service:latest
```
## Endpoints
POST /api/pedidos → Criar pedido
GET /api/pedidos → Listar pedidos
GET /api/pedidos/{id} → Consultar pedido específico

