¾)
jD:\Projetos\techChallenge - Fase 4\MicroServico-Pedido\Pedido\src\Pedido.Domain\ValueObjects\PedidoItem.cs
	namespace 	
Pedido
 
. 
Domain 
. 
ValueObjects $
{ 
public 

sealed 
class 

PedidoItem "
:# $
ValueObject% 0
{ 
public 
Guid 
Id 
{ 
get 
; 
private %
set& )
;) *
}+ ,
public 
Guid 
	ProdutoId 
{ 
get  #
;# $
private% ,
set- 0
;0 1
}2 3
public 
string 
ProdutoNome !
{" #
get$ '
;' (
private) 0
set1 4
;4 5
}6 7
public 
decimal 
	UnitPrice  
{! "
get# &
;& '
private( /
set0 3
;3 4
}5 6
public 
int 
Quant 
{ 
get 
; 
private  '
set( +
;+ ,
}- .
public$$ 
decimal$$ 
SubTotal$$ 
=>$$  "
Quant$$# (
*$$) *
	UnitPrice$$+ 4
;$$4 5
private)) 

PedidoItem)) 
()) 
))) 
{** 	
ProdutoNome++ 
=++ 
string++  
.++  !
Empty++! &
;++& '
},, 	
private11 

PedidoItem11 
(11 
Guid11 
id11  "
,11" #
Guid11$ (
	produtoId11) 2
,112 3
string114 :
produtoNome11; F
,11F G
decimal11H O
	unitPrice11P Y
,11Y Z
int11[ ^
quant11_ d
)11d e
{22 	
Id33 
=33 
id33 
;33 
	ProdutoId44 
=44 
	produtoId44 !
;44! "
ProdutoNome55 
=55 
produtoNome55 %
;55% &
	UnitPrice66 
=66 
	unitPrice66 !
;66! "
Quant77 
=77 
quant77 
;77 
}88 	
publicCC 
staticCC 

PedidoItemCC  
CreateCC! '
(CC' (
GuidCC( ,
	produtoIdCC- 6
,CC6 7
stringCC8 >
produtoNomeCC? J
,CCJ K
decimalCCL S
	unitPriceCCT ]
,CC] ^
intCC_ b
quantCCc h
)CCh i
{DD 	
ifEE 
(EE 
	produtoIdEE 
==EE 
GuidEE !
.EE! "
EmptyEE" '
)EE' (
throwFF 
newFF !
PedidoDomainExceptionFF /
(FF/ 0
$strFF0 O
)FFO P
;FFP Q
ifHH 
(HH 
stringHH 
.HH 
IsNullOrWhiteSpaceHH )
(HH) *
produtoNomeHH* 5
)HH5 6
)HH6 7
throwII 
newII !
PedidoDomainExceptionII /
(II/ 0
$strII0 Q
)IIQ R
;IIR S
ifKK 
(KK 
	unitPriceKK 
<=KK 
$numKK 
)KK 
throwLL 
newLL !
PedidoDomainExceptionLL /
(LL/ 0
$strLL0 Z
)LLZ [
;LL[ \
ifNN 
(NN 
quantNN 
<=NN 
$numNN 
)NN 
throwOO 
newOO !
PedidoDomainExceptionOO /
(OO/ 0
$strOO0 V
)OOV W
;OOW X
returnQQ 
newQQ 

PedidoItemQQ !
(QQ! "
GuidQQ" &
.QQ& '
NewGuidQQ' .
(QQ. /
)QQ/ 0
,QQ0 1
	produtoIdQQ2 ;
,QQ; <
produtoNomeQQ= H
,QQH I
	unitPriceQQJ S
,QQS T
quantQQU Z
)QQZ [
;QQ[ \
}RR 	
publicZZ 

PedidoItemZZ 
WithQuantityZZ &
(ZZ& '
intZZ' *
quantZZ+ 0
)ZZ0 1
{[[ 	
if\\ 
(\\ 
quant\\ 
<=\\ 
$num\\ 
)\\ 
throw]] 
new]] !
PedidoDomainException]] /
(]]/ 0
$str]]0 V
)]]V W
;]]W X
return__ 
new__ 

PedidoItem__ !
(__! "
Id__" $
,__$ %
	ProdutoId__& /
,__/ 0
ProdutoNome__1 <
,__< =
	UnitPrice__> G
,__G H
quant__I N
)__N O
;__O P
}`` 	
	protectedcc 
overridecc 
IEnumerablecc &
<cc& '
objectcc' -
?cc- .
>cc. /
GetAtomicValuescc0 ?
(cc? @
)cc@ A
{dd 	
yieldee 
returnee 
	ProdutoIdee "
;ee" #
yieldff 
returnff 
ProdutoNomeff $
;ff$ %
yieldgg 
returngg 
	UnitPricegg "
;gg" #
yieldhh 
returnhh 
Quanthh 
;hh 
}ii 	
}kk 
}ll ¹
lD:\Projetos\techChallenge - Fase 4\MicroServico-Pedido\Pedido\src\Pedido.Domain\ValueObjects\PedidoStatus.cs
	namespace 	
Pedido
 
. 
Domain 
. 
ValueObjects $
{ 
public 

enum 
PedidoStatus 
{ 
Pending 
= 
$num 
, 

Processing 
= 
$num 
, 
Ready 
= 
$num 
, 
	Completed 
= 
$num 
, 
	Cancelled 
= 
$num 
, 
Paid   
=   
$num   
,   
AwaitingPayment%% 
=%% 
$num%% 
}&& 
}'' Í)
rD:\Projetos\techChallenge - Fase 4\MicroServico-Pedido\Pedido\src\Pedido.Domain\Shared\ValueObjects\ValueObject.cs
	namespace		 	
Pedido		
 
.		 
Domain		 
.		 
Shared		 
.		 
ValueObjects		 +
{

 
public 

abstract 
class 
ValueObject %
:& '
IValueObject( 4
{ 
private 
static 
readonly  
ConcurrentDictionary  4
<4 5
Type5 9
,9 :
IReadOnlyCollection; N
<N O
PropertyInfoO [
>[ \
>\ ]
TypeProperties^ l
=m n
newo r
(r s
)s t
;t u
public 
override 
bool 
Equals #
(# $
object$ *
?* +
obj, /
)/ 0
{ 	
if 
( 
obj 
is 
null 
) 
return 
false 
; 
if 
( 
GetType 
( 
) 
!= 
obj  
.  !
GetType! (
(( )
)) *
)* +
return 
false 
; 
return 
GetAtomicValues "
(" #
)# $
.$ %
SequenceEqual% 2
(2 3
(3 4
(4 5
ValueObject5 @
)@ A
objA D
)D E
.E F
GetAtomicValuesF U
(U V
)V W
)W X
;X Y
} 	
public## 
override## 
int## 
GetHashCode## '
(##' (
)##( )
{$$ 	
return%% 
GetAtomicValues%% "
(%%" #
)%%# $
.&& 
Select&& 
(&& 
x&& 
=>&& 
x&& 
?&& 
.&&  
GetHashCode&&  +
(&&+ ,
)&&, -
??&&. 0
$num&&1 2
)&&2 3
.'' 
	Aggregate'' 
('' 
('' 
x'' 
,'' 
y''  
)''  !
=>''" $
x''% &
^''' (
y'') *
)''* +
;''+ ,
}(( 	
public-- 
static-- 
bool-- 
operator-- #
==--$ &
(--& '
ValueObject--' 2
?--2 3
left--4 8
,--8 9
ValueObject--: E
?--E F
right--G L
)--L M
{.. 	
if// 
(// 
left// 
is// 
null// 
&&// 
right//  %
is//& (
null//) -
)//- .
return00 
true00 
;00 
if22 
(22 
left22 
is22 
null22 
||22 
right22  %
is22& (
null22) -
)22- .
return33 
false33 
;33 
return55 
left55 
.55 
Equals55 
(55 
right55 $
)55$ %
;55% &
}66 	
public;; 
static;; 
bool;; 
operator;; #
!=;;$ &
(;;& '
ValueObject;;' 2
?;;2 3
left;;4 8
,;;8 9
ValueObject;;: E
?;;E F
right;;G L
);;L M
=>;;N P
!<< 
(<< 
left<< 
==<< 
right<< 
)<< 
;<< 
	protectedAA 
virtualAA 
IEnumerableAA %
<AA% &
objectAA& ,
?AA, -
>AA- .
GetAtomicValuesAA/ >
(AA> ?
)AA? @
{BB 	
returnCC 
GetPropertiesCC  
(CC  !
GetTypeCC! (
(CC( )
)CC) *
)CC* +
.DD 
SelectDD 
(DD 
pDD 
=>DD 
pDD 
.DD 
GetValueDD '
(DD' (
thisDD( ,
)DD, -
)DD- .
;DD. /
}EE 	
privateJJ 
staticJJ 
IReadOnlyCollectionJJ *
<JJ* +
PropertyInfoJJ+ 7
>JJ7 8
GetPropertiesJJ9 F
(JJF G
TypeJJG K
typeJJL P
)JJP Q
{KK 	
returnLL 
TypePropertiesLL !
.LL! "
GetOrAddLL" *
(LL* +
typeLL+ /
,LL/ 0
tLL1 2
=>LL3 5
tMM 
.MM 
GetPropertiesMM 
(MM  
BindingFlagsMM  ,
.MM, -
PublicMM- 3
|MM4 5
BindingFlagsMM6 B
.MMB C
InstanceMMC K
)MMK L
.NN 
WhereNN 
(NN 
pNN 
=>NN 
pNN 
.NN 
CanReadNN %
)NN% &
.OO 
OrderByOO 
(OO 
pOO 
=>OO 
pOO 
.OO  
NameOO  $
)OO$ %
.PP 
ToListPP 
(PP 
)PP 
.QQ 

AsReadOnlyQQ 
(QQ 
)QQ 
)QQ 
;QQ 
}RR 	
}SS 
}TT ê
sD:\Projetos\techChallenge - Fase 4\MicroServico-Pedido\Pedido\src\Pedido.Domain\Shared\ValueObjects\IValueObject.cs
	namespace 	
Pedido
 
. 
Domain 
. 
Shared 
. 
ValueObjects +
{ 
public		 

	interface		 
IValueObject		 !
{

 
bool 
Equals 
( 
object 
? 
other !
)! "
;" #
int 
GetHashCode 
( 
) 
; 
} 
} á
rD:\Projetos\techChallenge - Fase 4\MicroServico-Pedido\Pedido\src\Pedido.Domain\Shared\Repositories\IRepository.cs
	namespace		 	
Pedido		
 
.		 
Domain		 
.		 
Shared		 
.		 
Repositories		 +
{

 
public 

	interface 
IRepository  
<  !
T! "
>" #
where$ )
T* +
:, -
IEntity. 5
{ 
Task 
< 
T 
? 
> 
GetByIdAsync 
( 
Guid "
id# %
)% &
;& '
Task 
< 
T 
? 
> 
GetByIdAsync 
( 
Guid "
id# %
,% &
CancellationToken' 8
cancellationToken9 J
)J K
;K L
Task!! 
<!! 
IEnumerable!! 
<!! 
T!! 
>!! 
>!! 
GetAllAsync!! (
(!!( )
int!!) ,

pageNumber!!- 7
=!!8 9
$num!!: ;
,!!; <
int!!= @
pageSize!!A I
=!!J K
$num!!L N
)!!N O
;!!O P
Task** 
<** 
IEnumerable** 
<** 
T** 
>** 
>** 
GetAllAsync** (
(**( )
int**) ,

pageNumber**- 7
,**7 8
int**9 <
pageSize**= E
,**E F
CancellationToken**G X
cancellationToken**Y j
)**j k
;**k l
Task11 
<11 
T11 
>11 
CreateAsync11 
(11 
T11 
entity11 $
)11$ %
;11% &
Task77 
UpdateAsync77 
(77 
T77 
entity77 !
)77! "
;77" #
Task;; 
DeleteAsync;; 
(;; 
T;; 
entity;; !
);;! "
;;;" #
TaskBB 
DeleteAsyncBB 
(BB 
TBB 
entityBB !
,BB! "
CancellationTokenBB# 4
cancellationTokenBB5 F
)BBF G
;BBG H
}CC 
}DD ¥!
iD:\Projetos\techChallenge - Fase 4\MicroServico-Pedido\Pedido\src\Pedido.Domain\Shared\Entities\Entity.cs
	namespace 	
Pedido
 
. 
Domain 
. 
Shared 
. 
Entities '
{ 
public		 

abstract		 
class		 
Entity		  
:		! "
IEntity		# *
{

 
private 
static 
readonly 
TimeZoneInfo  ,
BrazilTimeZone- ;
=< =
TimeZoneInfo> J
.J K"
FindSystemTimeZoneByIdK a
(a b
$str	b ‚
)
‚ ƒ
;
ƒ „
public 
Guid 
Id 
{ 
get 
; 
	protected '
set( +
;+ ,
}- .
public 
DateTime 
	CreatedAt !
{" #
get$ '
;' (
private) 0
set1 4
;4 5
}6 7
public 
DateTime 
? 
	UpdatedAt "
{# $
get% (
;( )
private* 1
set2 5
;5 6
}7 8
	protected   
Entity   
(   
)   
{!! 	
Id"" 
="" 
Guid"" 
."" 
NewGuid"" 
("" 
)"" 
;""  
	CreatedAt## 
=## 
GetBrasilDateTime## )
(##) *
)##* +
;##+ ,
}$$ 	
public)) 
void)) 
SetUpdatedAt))  
())  !
)))! "
{** 	
	UpdatedAt++ 
=++ 
GetBrasilDateTime++ )
(++) *
)++* +
;+++ ,
},, 	
public22 
static22 
DateTime22 
GetBrasilDateTime22 0
(220 1
)221 2
{33 	
return44 
TimeZoneInfo44 
.44  
ConvertTimeFromUtc44  2
(442 3
DateTime443 ;
.44; <
UtcNow44< B
,44B C
BrazilTimeZone44D R
)44R S
;44S T
}55 	
public?? 
override?? 
bool?? 
Equals?? #
(??# $
object??$ *
???* +
obj??, /
)??/ 0
{@@ 	
ifAA 
(AA 
objAA 
isAA 
nullAA 
)AA 
returnBB 
falseBB 
;BB 
ifDD 
(DD 
objDD 
isDD 
notDD 
EntityDD !
entityDD" (
)DD( )
returnEE 
falseEE 
;EE 
ifGG 
(GG 
ReferenceEqualsGG 
(GG  
thisGG  $
,GG$ %
objGG& )
)GG) *
)GG* +
returnHH 
trueHH 
;HH 
returnJJ 
IdJJ 
==JJ 
entityJJ 
.JJ  
IdJJ  "
;JJ" #
}KK 	
publicRR 
overrideRR 
intRR 
GetHashCodeRR '
(RR' (
)RR( )
{SS 	
returnTT 
IdTT 
.TT 
GetHashCodeTT !
(TT! "
)TT" #
;TT# $
}UU 	
publicWW 
staticWW 
boolWW 
operatorWW #
==WW$ &
(WW& '
EntityWW' -
?WW- .
leftWW/ 3
,WW3 4
EntityWW5 ;
?WW; <
rightWW= B
)WWB C
{XX 	
returnYY 
leftYY 
?YY 
.YY 
EqualsYY 
(YY  
rightYY  %
)YY% &
??YY' )
rightYY* /
isYY0 2
nullYY3 7
;YY7 8
}ZZ 	
public\\ 
static\\ 
bool\\ 
operator\\ #
!=\\$ &
(\\& '
Entity\\' -
?\\- .
left\\/ 3
,\\3 4
Entity\\5 ;
?\\; <
right\\= B
)\\B C
{]] 	
return^^ 
!^^ 
(^^ 
left^^ 
==^^ 
right^^ "
)^^" #
;^^# $
}__ 	
}bb 
}dd ¯
kD:\Projetos\techChallenge - Fase 4\MicroServico-Pedido\Pedido\src\Pedido.Domain\Services\IPaymentService.cs
	namespace 	
Pedido
 
. 
Domain 
. 
Services  
{ 
public 

	interface 
IPaymentService $
{ 
Task 
< 
( 
string 
	QrCodeUrl 
, 
string  &
PreferenceId' 3
)3 4
>4 5
GenerateQrCodeAsync6 I
(I J
GuidJ N
orderIdO V
,V W
decimalX _
amount` f
)f g
;g h
Task 
< 
bool 
> 
ProcessPaymentAsync &
(& '
Guid' +
orderId, 3
,3 4
string5 ;
qrCode< B
)B C
;C D
} 
} ‹
tD:\Projetos\techChallenge - Fase 4\MicroServico-Pedido\Pedido\src\Pedido.Domain\Shared\Exceptions\DomainException.cs
	namespace 	
Pedido
 
. 
Domain 
. 
Shared 
. 

Exceptions )
{ 
public		 

abstract		 
class		 
DomainException		 )
:		* +
	Exception		, 5
{

 
	protected 
DomainException !
(! "
string" (
message) 0
)0 1
:2 3
base4 8
(8 9
message9 @
)@ A
{ 	
} 	
	protected 
DomainException !
(! "
string" (
message) 0
,0 1
	Exception2 ;
innerException< J
)J K
:L M
baseN R
(R S
messageS Z
,Z [
innerException\ j
)j k
{ 	
} 	
} 
} –
jD:\Projetos\techChallenge - Fase 4\MicroServico-Pedido\Pedido\src\Pedido.Domain\Shared\Entities\IEntity.cs
	namespace 	
Pedido
 
. 
Domain 
. 
Shared 
. 
Entities '
{ 
public		 

	interface		 
IEntity		 
{

 
Guid 
Id 
{ 
get 
; 
} 
DateTime 
	CreatedAt 
{ 
get  
;  !
}" #
DateTime 
? 
	UpdatedAt 
{ 
get !
;! "
}# $
void 
SetUpdatedAt 
( 
) 
; 
} 
} ”
pD:\Projetos\techChallenge - Fase 4\MicroServico-Pedido\Pedido\src\Pedido.Domain\Services\INotificationService.cs
	namespace 	
Pedido
 
. 
Domain 
. 
Services  
{ 
public		 

	interface		  
INotificationService		 )
{

 
Task )
NotifyPedidoStatusChangeAsync *
(* +
Entities+ 3
.3 4
Pedido4 :
pedido; A
,A B
PedidoStatusC O
previousStatusP ^
)^ _
;_ `
Task "
NotifyPedidoReadyAsync #
(# $
Entities$ ,
., -
Pedido- 3
pedido4 :
): ;
;; <
} 
} 
qD:\Projetos\techChallenge - Fase 4\MicroServico-Pedido\Pedido\src\Pedido.Domain\Repositories\IPedidoRepository.cs
	namespace 	
Pedido
 
. 
Domain 
. 
Repositories $
{ 
public 

	interface 
IPedidoRepository &
:' (
IRepository) 4
<4 5
Entities5 =
.= >
Pedido> D
>D E
{ 
Task 
< 
Entities 
. 
Pedido 
? 
> !
GetByIdWithItemsAsync 4
(4 5
Guid5 9
id: <
)< =
;= >
Task 
< 
IEnumerable 
< 
Entities !
.! "
Pedido" (
>( )
>) *
GetByClienteIdAsync+ >
(> ?
Guid? C

customerIdD N
,N O
intP S

pageNumberT ^
=_ `
$numa b
,b c
intd g
pageSizeh p
=q r
$nums u
)u v
;v w
Task$$ 
<$$ 
IEnumerable$$ 
<$$ 
Entities$$ !
.$$! "
Pedido$$" (
>$$( )
>$$) *
GetByStatusAsync$$+ ;
($$; <
PedidoStatus$$< H
status$$I O
,$$O P
int$$Q T

pageNumber$$U _
=$$` a
$num$$b c
,$$c d
int$$e h
pageSize$$i q
=$$r s
$num$$t v
)$$v w
;$$w x
Task.. 
<.. 
(.. 
IEnumerable.. 
<.. 
Entities.. "
..." #
Pedido..# )
>..) *
Pedidos..+ 2
,..2 3
int..4 7

TotalCount..8 B
)..B C
>..C D
GetPedidosAsync..E T
(..T U
int..U X

pageNumber..Y c
,..c d
int..e h
pageSize..i q
,..q r
Guid..s w
?..w x
	clienteId	..y ‚
=
..ƒ „
null
..… ‰
,
..‰ Š
PedidoStatus
..‹ —
?
..— ˜
status
..™ Ÿ
=
..  ¡
null
..¢ ¦
)
..¦ §
;
..§ ¨
Task66 
<66 
bool66 
>66 #
CustomerHasPedidosAsync66 *
(66* +
Guid66+ /

customerId660 :
,66: ;
CancellationToken66< M
cancellationToken66N _
)66_ `
;66` a
}77 
}88 †
sD:\Projetos\techChallenge - Fase 4\MicroServico-Pedido\Pedido\src\Pedido.Domain\Exceptions\PedidoDomainException.cs
	namespace 	
Pedido
 
. 
Domain 
. 

Exceptions "
{ 
public		 

class		 !
PedidoDomainException		 &
:		' (
DomainException		) 8
{

 
public !
PedidoDomainException $
($ %
string% +
message, 3
)3 4
:5 6
base7 ;
(; <
message< C
)C D
{ 	
} 	
} 
} Úa
bD:\Projetos\techChallenge - Fase 4\MicroServico-Pedido\Pedido\src\Pedido.Domain\Entities\Pedido.cs
	namespace 	
Pedido
 
. 
Domain 
. 
Entities  
{ 
public 

class 
Pedido 
: 
Entity  
{		 
public

 
Cliente

 
?

 
Cliente

 
{

  !
get

" %
;

% &
private

' .
set

/ 2
;

2 3
}

4 5
public 
Guid 
? 
	ClienteId 
{  
get! $
;$ %
private& -
set. 1
;1 2
}3 4
public 
IReadOnlyCollection "
<" #

PedidoItem# -
>- .
Items/ 4
=>5 7
_items8 >
.> ?

AsReadOnly? I
(I J
)J K
;K L
private 
readonly 
List 
< 

PedidoItem (
>( )
_items* 0
;0 1
public 
PedidoStatus 
Status "
{# $
get% (
;( )
private* 1
set2 5
;5 6
}7 8
public 
decimal 

TotalPrice !
{" #
get$ '
;' (
private) 0
set1 4
;4 5
}6 7
public 
string 
? 
QrCode 
{ 
get  #
;# $
private% ,
set- 0
;0 1
}2 3
public 
string 
? 
PreferenceId #
{$ %
get& )
;) *
private+ 2
set3 6
;6 7
}8 9
private 
Pedido 
( 
) 
: 
base 
(  
)  !
{ 	
_items 
= 
new 
List 
< 

PedidoItem (
>( )
() *
)* +
;+ ,
Status 
= 
PedidoStatus !
.! "
Pending" )
;) *

TotalPrice 
= 
$num 
; 
} 	
private 
Pedido 
( 
Guid 
? 
	clienteId &
,& '
List( ,
<, -

PedidoItem- 7
>7 8
items9 >
)> ?
:@ A
baseB F
(F G
)G H
{ 	
	ClienteId 
= 
	clienteId !
;! "
_items   
=   
items   
??   
new   !
List  " &
<  & '

PedidoItem  ' 1
>  1 2
(  2 3
)  3 4
;  4 5
Status!! 
=!! 
PedidoStatus!! !
.!!! "
Pending!!" )
;!!) *
CalculateTotalPrice"" 
(""  
)""  !
;""! "
}## 	
public%% 
static%% 
Pedido%% 
Create%% #
(%%# $
Guid%%$ (
?%%( )

customerId%%* 4
,%%4 5
List%%6 :
<%%: ;

PedidoItem%%; E
>%%E F
items%%G L
)%%L M
{&& 	
if'' 
('' 

customerId'' 
.'' 
HasValue'' #
&&''$ &

customerId''' 1
.''1 2
Value''2 7
==''8 :
Guid''; ?
.''? @
Empty''@ E
)''E F
throw(( 
new(( !
PedidoDomainException(( /
(((/ 0
$str((0 k
)((k l
;((l m
if** 
(** 
items** 
==** 
null** 
||**  
!**! "
items**" '
.**' (
Any**( +
(**+ ,
)**, -
)**- .
throw++ 
new++ !
PedidoDomainException++ /
(++/ 0
$str++0 V
)++V W
;++W X
return-- 
new-- 
Pedido-- 
(-- 

customerId-- (
,--( )
items--* /
)--/ 0
;--0 1
}.. 	
public00 
void00 
AddItem00 
(00 

PedidoItem00 &
item00' +
)00+ ,
{11 	
if22 
(22 
Status22 
!=22 
PedidoStatus22 &
.22& '
Pending22' .
)22. /
throw33 
new33 !
PedidoDomainException33 /
(33/ 0
$str330 r
)33r s
;33s t
if55 
(55 
item55 
==55 
null55 
)55 
throw66 
new66 !
PedidoDomainException66 /
(66/ 0
$str660 J
)66J K
;66K L
_items88 
.88 
Add88 
(88 
item88 
)88 
;88 
CalculateTotalPrice99 
(99  
)99  !
;99! "
SetUpdatedAt:: 
(:: 
):: 
;:: 
};; 	
public== 
bool== 

RemoveItem== 
(== 
Guid== #
	produtoId==$ -
)==- .
{>> 	
if?? 
(?? 
Status?? 
!=?? 
PedidoStatus?? &
.??& '
Pending??' .
)??. /
throw@@ 
new@@ !
PedidoDomainException@@ /
(@@/ 0
$str@@0 p
)@@p q
;@@q r
varBB 
itemBB 
=BB 
_itemsBB 
.BB 
FirstOrDefaultBB ,
(BB, -
iBB- .
=>BB/ 1
iBB2 3
.BB3 4
	ProdutoIdBB4 =
==BB> @
	produtoIdBBA J
)BBJ K
;BBK L
ifCC 
(CC 
itemCC 
==CC 
nullCC 
)CC 
returnDD 
falseDD 
;DD 
varFF 
removedFF 
=FF 
_itemsFF  
.FF  !
RemoveFF! '
(FF' (
itemFF( ,
)FF, -
;FF- .
ifGG 
(GG 
removedGG 
)GG 
{HH 
CalculateTotalPriceII #
(II# $
)II$ %
;II% &
SetUpdatedAtJJ 
(JJ 
)JJ 
;JJ 
}KK 
returnLL 
removedLL 
;LL 
}MM 	
publicOO 
voidOO 
UpdateStatusOO  
(OO  !
PedidoStatusOO! -
statusOO. 4
)OO4 5
{PP 	
ifQQ 
(QQ 
!QQ #
IsValidStatusTransitionQQ (
(QQ( )
StatusQQ) /
,QQ/ 0
statusQQ1 7
)QQ7 8
)QQ8 9
throwRR 
newRR !
PedidoDomainExceptionRR /
(RR/ 0
$"RR0 2
$strRR2 H
{RRH I
StatusRRI O
}RRO P
$strRRP V
{RRV W
statusRRW ]
}RR] ^
$strRR^ n
"RRn o
)RRo p
;RRp q
StatusTT 
=TT 
statusTT 
;TT 
SetUpdatedAtUU 
(UU 
)UU 
;UU 
}VV 	
publicXX 
voidXX 
	SetQrCodeXX 
(XX 
stringXX $
qrCodeXX% +
)XX+ ,
{YY 	
ifZZ 
(ZZ 
stringZZ 
.ZZ 
IsNullOrWhiteSpaceZZ )
(ZZ) *
qrCodeZZ* 0
)ZZ0 1
)ZZ1 2
throw[[ 
new[[ !
PedidoDomainException[[ /
([[/ 0
$str[[0 N
)[[N O
;[[O P
QrCode]] 
=]] 
qrCode]] 
;]] 
SetUpdatedAt^^ 
(^^ 
)^^ 
;^^ 
}__ 	
publicaa 
voidaa 
SetPreferenceIdaa #
(aa# $
stringaa$ *
preferenceIdaa+ 7
)aa7 8
{bb 	
ifcc 
(cc 
stringcc 
.cc 
IsNullOrWhiteSpacecc )
(cc) *
preferenceIdcc* 6
)cc6 7
)cc7 8
throwdd 
newdd !
PedidoDomainExceptiondd /
(dd/ 0
$strdd0 X
)ddX Y
;ddY Z
PreferenceIdff 
=ff 
preferenceIdff '
;ff' (
SetUpdatedAtgg 
(gg 
)gg 
;gg 
}hh 	
privatejj 
booljj #
IsValidStatusTransitionjj ,
(jj, -
PedidoStatusjj- 9
currentStatusjj: G
,jjG H
PedidoStatusjjI U
	newStatusjjV _
)jj_ `
{kk 	
returnll 
(ll 
currentStatusll !
,ll! "
	newStatusll# ,
)ll, -
switchll. 4
{mm 
(nn 
PedidoStatusnn 
.nn 
Pendingnn %
,nn% &
PedidoStatusnn' 3
.nn3 4

Processingnn4 >
)nn> ?
=>nn@ B
truennC G
,nnG H
(oo 
PedidoStatusoo 
.oo 

Processingoo (
,oo( )
PedidoStatusoo* 6
.oo6 7
Readyoo7 <
)oo< =
=>oo> @
trueooA E
,ooE F
(pp 
PedidoStatuspp 
.pp 
Readypp #
,pp# $
PedidoStatuspp% 1
.pp1 2
	Completedpp2 ;
)pp; <
=>pp= ?
truepp@ D
,ppD E
(qq 
PedidoStatusqq 
.qq 
Pendingqq %
,qq% &
PedidoStatusqq' 3
.qq3 4
	Cancelledqq4 =
)qq= >
=>qq? A
trueqqB F
,qqF G
(rr 
PedidoStatusrr 
.rr 

Processingrr (
,rr( )
PedidoStatusrr* 6
.rr6 7
	Cancelledrr7 @
)rr@ A
=>rrB D
truerrE I
,rrI J
(tt 
PedidoStatustt 
.tt 
Pendingtt %
,tt% &
PedidoStatustt' 3
.tt3 4
AwaitingPaymenttt4 C
)ttC D
=>ttE G
truettH L
,ttL M
(uu 
PedidoStatusuu 
.uu 
AwaitingPaymentuu -
,uu- .
PedidoStatusuu/ ;
.uu; <
Paiduu< @
)uu@ A
=>uuB D
trueuuE I
,uuI J
(vv 
PedidoStatusvv 
.vv 
AwaitingPaymentvv -
,vv- .
PedidoStatusvv/ ;
.vv; <
	Cancelledvv< E
)vvE F
=>vvG I
truevvJ N
,vvN O
(ww 
PedidoStatusww 
.ww 
Paidww "
,ww" #
PedidoStatusww$ 0
.ww0 1

Processingww1 ;
)ww; <
=>ww= ?
trueww@ D
,wwD E
(yy 
PedidoStatusyy 
.yy 
Pendingyy %
,yy% &
PedidoStatusyy' 3
.yy3 4
Paidyy4 8
)yy8 9
=>yy: <
trueyy= A
,yyA B
(zz 
PedidoStatuszz 
.zz 

Processingzz (
,zz( )
PedidoStatuszz* 6
.zz6 7
Pendingzz7 >
)zz> ?
=>zz@ B
truezzC G
,zzG H
var|| 
(|| 
current|| 
,|| 
next|| "
)||" #
when||$ (
current||) 0
==||1 3
next||4 8
=>||9 ;
true||< @
,||@ A
_}} 
=>}} 
false}} 
}~~ 
;~~ 
} 	
private
 
void
 !
CalculateTotalPrice
 (
(
( )
)
) *
{
‚‚ 	

TotalPrice
ƒƒ 
=
ƒƒ 
_items
ƒƒ 
.
ƒƒ  
Sum
ƒƒ  #
(
ƒƒ# $
item
ƒƒ$ (
=>
ƒƒ) +
item
ƒƒ, 0
.
ƒƒ0 1
SubTotal
ƒƒ1 9
)
ƒƒ9 :
;
ƒƒ: ;
}
„„ 	
public
†† 
void
†† 
SetStatusDireto
†† #
(
††# $
PedidoStatus
††$ 0
status
††1 7
)
††7 8
{
‡‡ 	
Status
ˆˆ 
=
ˆˆ 
status
ˆˆ 
;
ˆˆ 
}
‰‰ 	
}
ŠŠ 
}‹‹ ‚"
mD:\Projetos\techChallenge - Fase 4\MicroServico-Pedido\Pedido\src\Pedido.Domain\Customer\ValueObjects\Name.cs
	namespace		 	
Pedido		
 
.		 
Domain		 
.		 
Custumer		  
.		  !
ValueObjects		! -
{

 
public 

record 
Name 
{ 
private 
const 
int 

MIN_LENGTH $
=% &
$num' (
;( )
private 
const 
int 

MAX_LENGTH $
=% &
$num' *
;* +
private 
static 
readonly 
Regex  %

NAME_REGEX& 0
=1 2
new3 6
(6 7
$str !
,! "
RegexOptions 
. 
Compiled !
)! "
;" #
public 
string 
Value 
{ 
get !
;! "
}# $
private 
Name 
( 
string 
value !
)! "
{ 	
Value 
= 
value 
; 
} 	
public'' 
static'' 
Name'' 
Create'' !
(''! "
string''" (
name'') -
)''- .
{(( 	
if)) 
()) 
string)) 
.)) 
IsNullOrWhiteSpace)) )
())) *
name))* .
))). /
)))/ 0
throw** 
new** "
ClienteDomainException** 0
(**0 1
$str**1 L
)**L M
;**M N
name,, 
=,, 
name,, 
.,, 
Trim,, 
(,, 
),, 
;,, 
if.. 
(.. 
name.. 
... 
Length.. 
<.. 

MIN_LENGTH.. (
)..( )
throw// 
new// "
ClienteDomainException// 0
(//0 1
$"//1 3
$str//3 M
{//M N

MIN_LENGTH//N X
}//X Y
$str//Y d
"//d e
)//e f
;//f g
if11 
(11 
name11 
.11 
Length11 
>11 

MAX_LENGTH11 (
)11( )
throw22 
new22 "
ClienteDomainException22 0
(220 1
$"221 3
$str223 P
{22P Q

MAX_LENGTH22Q [
}22[ \
$str22\ g
"22g h
)22h i
;22i j
if44 
(44 
!44 

NAME_REGEX44 
.44 
IsMatch44 #
(44# $
name44$ (
)44( )
)44) *
throw55 
new55 "
ClienteDomainException55 0
(550 1
$str551 U
)55U V
;55V W
var88 

properName88 
=88 
string88 #
.88# $
Join88$ (
(88( )
$str88) ,
,88, -
name99 
.99 
Split99 
(99 
$char99 
)99 
.:: 
Where:: 
(:: 
x:: 
=>:: 
!::  !
string::! '
.::' (
IsNullOrWhiteSpace::( :
(::: ;
x::; <
)::< =
)::= >
.;; 
Select;; 
(;; 
x;; 
=>;;  
char;;! %
.;;% &
ToUpper;;& -
(;;- .
x;;. /
[;;/ 0
$num;;0 1
];;1 2
);;2 3
+;;4 5
x;;6 7
[;;7 8
$num;;8 9
..;;9 ;
];;; <
.;;< =
ToLower;;= D
(;;D E
);;E F
);;F G
);;G H
;;;H I
return== 
new== 
Name== 
(== 

properName== &
)==& '
;==' (
}>> 	
publicCC 
overrideCC 
stringCC 
ToStringCC '
(CC' (
)CC( )
=>CC* ,
ValueCC- 2
;CC2 3
publicHH 
staticHH 
implicitHH 
operatorHH '
stringHH( .
(HH. /
NameHH/ 3
nameHH4 8
)HH8 9
=>HH: <
nameHH= A
.HHA B
ValueHHB G
;HHG H
}II 
}JJ •
}D:\Projetos\techChallenge - Fase 4\MicroServico-Pedido\Pedido\src\Pedido.Domain\Customer\Exceptions\ClienteDomainException.cs
	namespace 	
Pedido
 
. 
Domain 
. 
Custumer  
.  !

Exceptions! +
{		 
public

 

class

 "
ClienteDomainException

 '
:

( )
DomainException

* 9
{ 
public "
ClienteDomainException %
(% &
string& ,
message- 4
)4 5
:6 7
base8 <
(< =
message= D
)D E
{ 	
} 	
public "
ClienteDomainException %
(% &
string& ,
message- 4
,4 5
	Exception6 ?
innerException@ N
)N O
:P Q
baseR V
(V W
messageW ^
,^ _
innerException` n
)n o
{ 	
} 	
} 
} Õ
lD:\Projetos\techChallenge - Fase 4\MicroServico-Pedido\Pedido\src\Pedido.Domain\Customer\Entities\Cliente.cs
	namespace		 	
Pedido		
 
.		 
Domain		 
.		 
Custumer		  
.		  !
Entities		! )
{

 
public 

class 
Cliente 
{ 
public 
Guid 
Id 
{ 
get 
; 
} 
public 
string 
Cpf 
{ 
get 
;  
}! "
public 
Name 
Nome 
{ 
get 
; 
private  '
set( +
;+ ,
}- .
	protected 
Cliente 
( 
) 
{ 
} 
public 
Cliente 
( 
Guid 
id 
, 
string  &
cpf' *
,* +
Name, 0
nome1 5
)5 6
{ 	
Id 
= 
id 
; 
Cpf 
= 
cpf 
; 
Nome 
= 
nome 
?? 
throw  
new! $!
ArgumentNullException% :
(: ;
nameof; A
(A B
nomeB F
)F G
)G H
;H I
} 	
private 
Cliente 
( 
Name 
name !
)! "
{ 	
Id 
= 
Guid 
. 
NewGuid 
( 
) 
;  
Nome 
= 
name 
; 
} 	
public   
static   
Cliente   
Create   $
(  $ %
Name  % )
name  * .
)  . /
{!! 	
return"" 
new"" 
Cliente"" 
("" 
name"" #
)""# $
;""$ %
}## 	
}%% 
}&& 