Ç)
ÉC:\Users\armad\Documentos\Estudio\Proyectos\infobitservicebackend\src\Application\LogsConversaciones\Queries\LogsConversacionDto.cs
	namespace 	!
infobitservicebackend
 
.  
Application  +
.+ ,
LogsConversaciones, >
.> ?
Queries? F
{ 
[		 
SwaggerSchema		 
(		 
Title		 
=		 
$str		 1
,		1 2
Description		3 >
=		? @
$str		A ~
)		~ 
]			 Ä
public

 

class

 
LogsConversacionDto

 $
{ 
[ 	
SwaggerSchema	 
( 
Description "
=# $
$str% l
)l m
]m n
public 
required 
string 
PartitionKey +
{, -
get. 1
;1 2
set3 6
;6 7
}8 9
[ 	
SwaggerSchema	 
( 
Description "
=# $
$str% d
)d e
]e f
public 
required 
string 
RowKey %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
[ 	
SwaggerSchema	 
( 
Description "
=# $
$str% c
)c d
]d e
public 
DateTimeOffset 
? 
	Timestamp (
{) *
get+ .
;. /
set0 3
;3 4
}5 6
[!! 	
SwaggerSchema!!	 
(!! 
Description!! "
=!!# $
$str!!% M
)!!M N
]!!N O
public"" 
required"" 
string"" 
Channel"" &
{""' (
get"") ,
;"", -
set"". 1
;""1 2
}""3 4
['' 	
SwaggerSchema''	 
('' 
Description'' "
=''# $
$str''% d
)''d e
]''e f
public(( 
required(( 
DateTime((  
DateTime((! )
{((* +
get((, /
;((/ 0
set((1 4
;((4 5
}((6 7
[-- 	
SwaggerSchema--	 
(-- 
Description-- "
=--# $
$str--% d
)--d e
]--e f
public.. 
required.. 
string.. 
	Direction.. (
{..) *
get..+ .
;... /
set..0 3
;..3 4
}..5 6
[33 	
SwaggerSchema33	 
(33 
Description33 "
=33# $
$str33% T
)33T U
]33U V
public44 
required44 
string44 
From44 #
{44$ %
get44& )
;44) *
set44+ .
;44. /
}440 1
[99 	
SwaggerSchema99	 
(99 
Description99 "
=99# $
$str99% P
)99P Q
]99Q R
public:: 
required:: 
string:: 
SentBy:: %
{::& '
get::( +
;::+ ,
set::- 0
;::0 1
}::2 3
[?? 	
SwaggerSchema??	 
(?? 
Description?? "
=??# $
$str??% L
)??L M
]??M N
public@@ 
required@@ 
string@@ 
Text@@ #
{@@$ %
get@@& )
;@@) *
set@@+ .
;@@. /
}@@0 1
[EE 	
SwaggerSchemaEE	 
(EE 
DescriptionEE "
=EE# $
$strEE% W
)EEW X
]EEX Y
publicFF 
requiredFF 
stringFF 
ToFF !
{FF" #
getFF$ '
;FF' (
setFF) ,
;FF, -
}FF. /
[KK 	
SwaggerSchemaKK	 
(KK 
DescriptionKK "
=KK# $
$strKK% J
)KKJ K
]KKK L
publicLL 
requiredLL 
stringLL 
TypeLL #
{LL$ %
getLL& )
;LL) *
setLL+ .
;LL. /
}LL0 1
[QQ 	
SwaggerSchemaQQ	 
(QQ 
DescriptionQQ "
=QQ# $
$strQQ% m
)QQm n
]QQn o
publicRR 
requiredRR 
stringRR 
ConversationIDRR -
{RR. /
getRR0 3
;RR3 4
setRR5 8
;RR8 9
}RR: ;
[WW 	
SwaggerSchemaWW	 
(WW 
DescriptionWW "
=WW# $
$strWW% W
)WWW X
]WWX Y
publicXX 
requiredXX 
stringXX 
	NumeroCelXX (
{XX) *
getXX+ .
;XX. /
setXX0 3
;XX3 4
}XX5 6
private]] 
class]] 
Mapping]] 
:]] 
Profile]]  '
{^^ 	
public__ 
Mapping__ 
(__ 
)__ 
{`` 
	CreateMapaa 
<aa 
LogsConversacionaa *
,aa* +
LogsConversacionDtoaa, ?
>aa? @
(aa@ A
)aaA B
;aaB C
}bb 
}cc 	
}dd 
}ee ≈)
ìC:\Users\armad\Documentos\Estudio\Proyectos\infobitservicebackend\src\Application\LogsConversaciones\Queries\GetLogsConversacionesQueryValidator.cs
	namespace 	!
infobitservicebackend
 
.  
Application  +
.+ ,
LogsConversaciones, >
.> ?
Queries? F
;F G
public 
class /
#GetLogsConversacionesQueryValidator 0
:1 2
AbstractValidator3 D
<D E&
GetLogsConversacionesQueryE _
>_ `
{ 
public 
/
#GetLogsConversacionesQueryValidator .
(. /
)/ 0
{		 
RuleFor

 
(

 
x

 
=>

 
x

 
.

 
PartitionKey

 #
)

# $
. 
NotEmpty 
( 
) 
. 
WithMessage #
(# $
$str$ O
)O P
. 
MinimumLength 
( 
$num 
) 
. 
WithMessage )
() *
$str* g
)g h
. 
Matches 
( 
$str 
)  
.  !
WithMessage! ,
(, -
$str- j
)j k
;k l
RuleFor 
( 
x 
=> 
x 
. 

FechaDesde !
)! "
. 
Must 
( 
BeValidDateFormat #
)# $
. 
When 
( 
x 
=> 
! 
string 
. 
IsNullOrEmpty ,
(, -
x- .
.. /

FechaDesde/ 9
)9 :
): ;
. 
WithMessage 
( 
$str }
)} ~
;~ 
RuleFor 
( 
x 
=> 
x 
. 

FechaHasta !
)! "
. 
Must 
( 
BeValidDateFormat #
)# $
. 
When 
( 
x 
=> 
! 
string 
. 
IsNullOrEmpty ,
(, -
x- .
.. /

FechaHasta/ 9
)9 :
): ;
. 
WithMessage 
( 
$str }
)} ~
;~ 
RuleFor 
( 
x 
=> 
x 
) 
. 
Must 
( 
x 
=> 
BeValidRange #
(# $
x$ %
.% &

FechaDesde& 0
,0 1
x2 3
.3 4

FechaHasta4 >
)> ?
)? @
. 
When 
( 
x 
=> 
! 
string 
. 
IsNullOrEmpty ,
(, -
x- .
.. /

FechaDesde/ 9
)9 :
&&; =
!> ?
string? E
.E F
IsNullOrEmptyF S
(S T
xT U
.U V

FechaHastaV `
)` a
)a b
. 
WithMessage 
( 
$str N
)N O
;O P
} 
private 
static 
bool 
BeValidDateFormat )
() *
string* 0
?0 1
date2 6
)6 7
{   
return!! 
DateTime!! 
.!! 
TryParseExact!! %
(!!% &
date!!& *
,!!* +
$str!!, A
,!!A B
CultureInfo!!C N
.!!N O
InvariantCulture!!O _
,!!_ `
DateTimeStyles!!a o
.!!o p
None!!p t
,!!t u
out!!v y
_!!z {
)!!{ |
;!!| }
}"" 
private$$ 
static$$ 
bool$$ 
BeValidRange$$ $
($$$ %
string$$% +
?$$+ ,

fechaDesde$$- 7
,$$7 8
string$$9 ?
?$$? @

fechaHasta$$A K
)$$K L
{%% 
if&& 

(&& 
DateTime&& 
.&& 
TryParseExact&& "
(&&" #

fechaDesde&&# -
,&&- .
$str&&/ D
,&&D E
CultureInfo&&F Q
.&&Q R
InvariantCulture&&R b
,&&b c
DateTimeStyles&&d r
.&&r s
None&&s w
,&&w x
out&&y |
var	&&} Ä
desde
&&Å Ü
)
&&Ü á
&&
&&à ä
DateTime'' 
.'' 
TryParseExact'' "
(''" #

fechaHasta''# -
,''- .
$str''/ D
,''D E
CultureInfo''F Q
.''Q R
InvariantCulture''R b
,''b c
DateTimeStyles''d r
.''r s
None''s w
,''w x
out''y |
var	''} Ä
hasta
''Å Ü
)
''Ü á
)
''á à
{(( 	
return)) 
desde)) 
<=)) 
hasta)) !
;))! "
}** 	
return,, 
true,, 
;,, 
}-- 
}.. Ö@
ëC:\Users\armad\Documentos\Estudio\Proyectos\infobitservicebackend\src\Application\LogsConversaciones\Queries\GetLogsConversacionesQueryHandler.cs
	namespace 	!
infobitservicebackend
 
.  
Application  +
.+ ,
LogsConversaciones, >
.> ?
Queries? F
{ 
public		 

class		 -
!GetLogsConversacionesQueryHandler		 2
:		3 4
IRequestHandler		5 D
<		D E&
GetLogsConversacionesQuery		E _
,		_ `
PaginatedList		a n
<		n o 
LogsConversacionDto			o Ç
>
		Ç É
>
		É Ñ
{

 
private 
readonly  
ITableStorageService - 
_tableStorageService. B
;B C
private 
readonly 
IMapper  
_mapper! (
;( )
public -
!GetLogsConversacionesQueryHandler 0
(0 1 
ITableStorageService1 E
tableStorageServiceF Y
,Y Z
IMapper[ b
mapperc i
)i j
{ 	 
_tableStorageService  
=! "
tableStorageService# 6
;6 7
_mapper 
= 
mapper 
; 
} 	
public 
async 
Task 
< 
PaginatedList '
<' (
LogsConversacionDto( ;
>; <
>< =
Handle> D
(D E&
GetLogsConversacionesQueryE _
request` g
,g h
CancellationTokeni z
cancellationToken	{ å
)
å ç
{ 	
DateTime 
? 

fechaDesde  
=! "
	ParseDate# ,
(, -
request- 4
.4 5

FechaDesde5 ?
)? @
;@ A
DateTime 
? 

fechaHasta  
=! "
	ParseDate# ,
(, -
request- 4
.4 5

FechaHasta5 ?
)? @
;@ A
var 
entities 
= 
await   
_tableStorageService! 5
.5 6*
GetEntitiesByPartitionKeyAsync6 T
<T U
LogsConversacionU e
>e f
(f g
$strg {
,{ |
request	} Ñ
.
Ñ Ö
PartitionKey
Ö ë
)
ë í
;
í ì
var 
filteredEntities  
=! "
entities# +
.+ ,
Where, 1
(1 2
e2 3
=>4 6
{ 
DateTime 
fechaRowKey $
=% &#
ExtraerFechaDesdeRowKey' >
(> ?
e? @
.@ A
RowKeyA G
)G H
;H I
bool   
isValid   
=   
true   #
;  # $
if"" 
("" 

fechaDesde"" 
."" 
HasValue"" '
)""' (
isValid## 
&=## 
fechaRowKey## *
>=##+ -

fechaDesde##. 8
.##8 9
Value##9 >
;##> ?
if%% 
(%% 

fechaHasta%% 
.%% 
HasValue%% '
)%%' (
isValid&& 
&=&& 
fechaRowKey&& *
<=&&+ -

fechaHasta&&. 8
.&&8 9
Value&&9 >
;&&> ?
if(( 
((( 
!(( 
string(( 
.(( 
IsNullOrWhiteSpace(( .
(((. /
request((/ 6
.((6 7
Channel((7 >
)((> ?
)((? @
isValid)) 
&=)) 
e))  
.))  !
Channel))! (
.))( )
Equals))) /
())/ 0
request))0 7
.))7 8
Channel))8 ?
,))? @
StringComparison))A Q
.))Q R
OrdinalIgnoreCase))R c
)))c d
;))d e
return++ 
isValid++ 
;++ 
},, 
),, 
;,, 
var.. 
mappedEntities.. 
=..  
_mapper..! (
...( )
Map..) ,
<.., -
List..- 1
<..1 2
LogsConversacionDto..2 E
>..E F
>..F G
(..G H
filteredEntities..H X
)..X Y
.// 
OrderBy// 
(// 
e// 
=>//  #
ExtraerFechaDesdeRowKey//! 8
(//8 9
e//9 :
.//: ;
RowKey//; A
)//A B
)//B C
.00 
ThenBy00 
(00 
e00 
=>00  
ExtraerIdDesdeRowKey00  4
(004 5
e005 6
.006 7
RowKey007 =
)00= >
)00> ?
;00? @
if22 
(22 
!22 
request22 
.22 

PageNumber22 #
.22# $
HasValue22$ ,
||22- /
!220 1
request221 8
.228 9
PageSize229 A
.22A B
HasValue22B J
)22J K
{33 
return44 
new44 
PaginatedList44 (
<44( )
LogsConversacionDto44) <
>44< =
(44= >
mappedEntities55 "
.55" #
ToList55# )
(55) *
)55* +
,55+ ,
mappedEntities55- ;
.55; <
Count55< A
(55A B
)55B C
,55C D
$num55E F
,55F G
mappedEntities55H V
.55V W
Count55W \
(55\ ]
)55] ^
)55^ _
;55_ `
}66 
return88 
PaginatedList88  
<88  !
LogsConversacionDto88! 4
>884 5
.885 6
Create886 <
(88< =
mappedEntities99 
,99 
request:: 
.:: 

PageNumber:: "
.::" #
Value::# (
,::( )
request;; 
.;; 
PageSize;;  
.;;  !
Value;;! &
);;& '
;;;' (
}== 	
private?? 
DateTime?? 
??? 
	ParseDate?? #
(??# $
string??$ *
???* +
date??, 0
)??0 1
{@@ 	
ifAA 
(AA 
DateTimeAA 
.AA 
TryParseExactAA &
(AA& '
dateAA' +
,AA+ ,
$strAA- B
,AAB C
CultureInfoAAD O
.AAO P
InvariantCultureAAP `
,AA` a
DateTimeStylesAAb p
.AAp q
NoneAAq u
,AAu v
outAAw z
varAA{ ~

parsedDate	AA â
)
AAâ ä
)
AAä ã
{BB 
returnCC 

parsedDateCC !
;CC! "
}DD 
returnEE 
nullEE 
;EE 
}FF 	
privateHH 
DateTimeHH #
ExtraerFechaDesdeRowKeyHH 0
(HH0 1
stringHH1 7
rowKeyHH8 >
)HH> ?
{II 	
stringJJ 
fechaStrJJ 
=JJ 
rowKeyJJ $
.JJ$ %
	SubstringJJ% .
(JJ. /
$numJJ/ 0
,JJ0 1
$numJJ2 4
)JJ4 5
;JJ5 6
returnKK 
DateTimeKK 
.KK 

ParseExactKK &
(KK& '
fechaStrKK' /
,KK/ 0
$strKK1 A
,KKA B
CultureInfoKKC N
.KKN O
InvariantCultureKKO _
)KK_ `
;KK` a
}LL 	
privateNN 
intNN  
ExtraerIdDesdeRowKeyNN (
(NN( )
stringNN) /
rowKeyNN0 6
)NN6 7
{OO 	
stringPP 
idStrPP 
=PP 
rowKeyPP !
.PP! "
	SubstringPP" +
(PP+ ,
$numPP, .
)PP. /
;PP/ 0
ifQQ 
(QQ 
intQQ 
.QQ 
TryParseQQ 
(QQ 
idStrQQ "
,QQ" #
outQQ$ '
intQQ( +
idQQ, .
)QQ. /
)QQ/ 0
returnRR 
idRR 
;RR 
returnTT 
$numTT 
;TT 
}UU 	
}VV 
}WW ¿
äC:\Users\armad\Documentos\Estudio\Proyectos\infobitservicebackend\src\Application\LogsConversaciones\Queries\GetLogsConversacionesQuery.cs
	namespace 	!
infobitservicebackend
 
.  
Application  +
.+ ,
LogsConversaciones, >
.> ?
Queries? F
{ 
public 

class &
GetLogsConversacionesQuery +
:, -
IRequest. 6
<6 7
PaginatedList7 D
<D E
LogsConversacionDtoE X
>X Y
>Y Z
{		 
public

 
string

 
PartitionKey

 "
{

# $
get

% (
;

( )
set

* -
;

- .
}

/ 0
public 
string 
? 

FechaDesde !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
string 
? 

FechaHasta !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
string 
? 
Channel 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
int 
? 

PageNumber 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
int 
? 
PageSize 
{ 
get "
;" #
set$ '
;' (
}) *
public &
GetLogsConversacionesQuery )
() *
string* 0
partitionKey1 =
,= >
string? E
?E F

fechaDesdeG Q
,Q R
stringS Y
?Y Z

fechaHasta[ e
,e f
stringg m
?m n
channelo v
,v w
intx {
?{ |

pageNumber	} á
,
á à
int
â å
?
å ç
pageSize
é ñ
)
ñ ó
{ 	
PartitionKey 
= 
partitionKey '
;' (

FechaDesde 
= 

fechaDesde #
;# $

FechaHasta 
= 

fechaHasta #
;# $
Channel 
= 
channel 
; 

PageNumber 
= 

pageNumber #
;# $
PageSize 
= 
pageSize 
;  
} 	
} 
} ≈
hC:\Users\armad\Documentos\Estudio\Proyectos\infobitservicebackend\src\Application\DependencyInjection.cs
	namespace 	
	Microsoft
 
. 

Extensions 
. 
DependencyInjection 2
;2 3
public 
static 
class 
DependencyInjection '
{		 
public

 

static

 
void

 "
AddApplicationServices

 -
(

- .
this

. 2#
IHostApplicationBuilder

3 J
builder

K R
)

R S
{ 
builder 
. 
Services 
. 
AddAutoMapper &
(& '
Assembly' /
./ 0 
GetExecutingAssembly0 D
(D E
)E F
)F G
;G H
builder 
. 
Services 
. %
AddValidatorsFromAssembly 2
(2 3
Assembly3 ;
.; < 
GetExecutingAssembly< P
(P Q
)Q R
)R S
;S T
builder 
. 
Services 
. /
#AddValidatorsFromAssemblyContaining <
<< =/
#GetLogsConversacionesQueryValidator= `
>` a
(a b
)b c
;c d
builder 
. 
Services 
. 

AddMediatR #
(# $
cfg$ '
=>( *
{ 	
cfg 
. (
RegisterServicesFromAssembly ,
(, -
Assembly- 5
.5 6 
GetExecutingAssembly6 J
(J K
)K L
)L M
;M N
cfg 
. 
AddBehavior 
( 
typeof "
(" #
IPipelineBehavior# 4
<4 5
,5 6
>6 7
)7 8
,8 9
typeof: @
(@ A'
UnhandledExceptionBehaviourA \
<\ ]
,] ^
>^ _
)_ `
)` a
;a b
cfg 
. 
AddBehavior 
( 
typeof "
(" #
IPipelineBehavior# 4
<4 5
,5 6
>6 7
)7 8
,8 9
typeof: @
(@ A"
AuthorizationBehaviourA W
<W X
,X Y
>Y Z
)Z [
)[ \
;\ ]
cfg 
. 
AddBehavior 
( 
typeof "
(" #
IPipelineBehavior# 4
<4 5
,5 6
>6 7
)7 8
,8 9
typeof: @
(@ A
ValidationBehaviourA T
<T U
,U V
>V W
)W X
)X Y
;Y Z
cfg 
. 
AddBehavior 
( 
typeof "
(" #
IPipelineBehavior# 4
<4 5
,5 6
>6 7
)7 8
,8 9
typeof: @
(@ A 
PerformanceBehaviourA U
<U V
,V W
>W X
)X Y
)Y Z
;Z [
} 	
)	 

;
 
} 
} c
aC:\Users\armad\Documentos\Estudio\Proyectos\infobitservicebackend\src\Application\GlobalUsings.cs’
pC:\Users\armad\Documentos\Estudio\Proyectos\infobitservicebackend\src\Application\Common\Models\PaginatedList.cs
	namespace 	!
infobitservicebackend
 
.  
Application  +
.+ ,
Common, 2
.2 3
Models3 9
;9 :
public 
class 
PaginatedList 
< 
T 
> 
{ 
public 

IReadOnlyCollection 
< 
T  
>  !
Items" '
{( )
get* -
;- .
}/ 0
public 

int 

PageNumber 
{ 
get 
;  
}! "
public 

int 

TotalPages 
{ 
get 
;  
}! "
public 

int 

TotalCount 
{ 
get 
;  
}! "
public

 

PaginatedList

 
(

 
IReadOnlyCollection

 ,
<

, -
T

- .
>

. /
items

0 5
,

5 6
int

7 :
count

; @
,

@ A
int

B E

pageNumber

F P
,

P Q
int

R U
pageSize

V ^
)

^ _
{ 

PageNumber 
= 

pageNumber 
;  

TotalPages 
= 
( 
int 
) 
Math 
. 
Ceiling &
(& '
count' ,
/- .
(/ 0
double0 6
)6 7
pageSize7 ?
)? @
;@ A

TotalCount 
= 
count 
; 
Items 
= 
items 
; 
} 
public 

bool 
HasPreviousPage 
=>  "

PageNumber# -
>. /
$num0 1
;1 2
public 

bool 
HasNextPage 
=> 

PageNumber )
<* +

TotalPages, 6
;6 7
public 

static 
PaginatedList 
<  
T  !
>! "
Create# )
() *
IEnumerable* 5
<5 6
T6 7
>7 8
source9 ?
,? @
intA D

pageNumberE O
,O P
intQ T
pageSizeU ]
)] ^
{ 
var 
count 
= 
source 
. 
Count  
(  !
)! "
;" #
var 
items 
= 
source 
. 
Skip 
(  
(  !

pageNumber! +
-, -
$num. /
)/ 0
*1 2
pageSize3 ;
); <
.< =
Take= A
(A B
pageSizeB J
)J K
.K L
ToListL R
(R S
)S T
;T U
return 
new 
PaginatedList  
<  !
T! "
>" #
(# $
items$ )
,) *
count+ 0
,0 1

pageNumber2 <
,< =
pageSize> F
)F G
;G H
} 
} ¸	
vC:\Users\armad\Documentos\Estudio\Proyectos\infobitservicebackend\src\Application\Common\Mappings\MappingExtensions.cs
	namespace 	!
infobitservicebackend
 
.  
Application  +
.+ ,
Common, 2
.2 3
Mappings3 ;
;; <
public 
static 
class 
MappingExtensions %
{ 
public 

static 
Task 
< 
List 
< 
TDestination (
>( )
>) *
ProjectToListAsync+ =
<= >
TDestination> J
>J K
(K L
thisL P

IQueryableQ [
	queryable\ e
,e f"
IConfigurationProviderg }
configuration	~ ã
)
ã å
where
ç í
TDestination
ì ü
:
† °
class
¢ ß
=> 

	queryable 
. 
	ProjectTo 
< 
TDestination +
>+ ,
(, -
configuration- :
): ;
.; <
AsNoTracking< H
(H I
)I J
.J K
ToListAsyncK V
(V W
)W X
;X Y
}		 È
qC:\Users\armad\Documentos\Estudio\Proyectos\infobitservicebackend\src\Application\Common\Interfaces\ITokenInfo.cs
	namespace 	!
infobitservicebackend
 
.  
Application  +
.+ ,
Common, 2
.2 3

Interfaces3 =
;= >
public 
	interface 

ITokenInfo 
{ 
string 

?
 
RawToken 
{ 
get 
; 
} 
bool 
HasValidToken	 
{ 
get 
; 
} 
} ¯

{C:\Users\armad\Documentos\Estudio\Proyectos\infobitservicebackend\src\Application\Common\Interfaces\ITableStorageService.cs
	namespace 	!
infobitservicebackend
 
.  
Application  +
.+ ,
Common, 2
.2 3

Interfaces3 =
{ 
public 

	interface  
ITableStorageService )
{ 
Task 
< 
List 
< 
T 
> 
> 
GetEntitiesAsync &
<& '
T' (
>( )
() *
string* 0
	tableName1 :
): ;
where< A
TB C
:D E
classF K
,K L
ITableEntityM Y
,Y Z
new[ ^
(^ _
)_ `
;` a
Task		 
<		 
List		 
<		 
T		 
>		 
>		 *
GetEntitiesByPartitionKeyAsync		 4
<		4 5
T		5 6
>		6 7
(		7 8
string		8 >
	tableName		? H
,		H I
string		J P
partitionKey		Q ]
)		] ^
where		_ d
T		e f
:		g h
class		i n
,		n o
ITableEntity		p |
,		| }
new			~ Å
(
		Å Ç
)
		Ç É
;
		É Ñ
}

 
} ‡
wC:\Users\armad\Documentos\Estudio\Proyectos\infobitservicebackend\src\Application\Common\Interfaces\IIdentityService.cs
	namespace 	!
infobitservicebackend
 
.  
Application  +
.+ ,
Common, 2
.2 3

Interfaces3 =
{ 
public 

	interface 
IIdentityService %
{ 
Task 
< 
string 
> !
GenerateJwtTokenAsync *
(* +
string+ 1
apiKey2 8
)8 9
;9 :
} 
} ≥
zC:\Users\armad\Documentos\Estudio\Proyectos\infobitservicebackend\src\Application\Common\Exceptions\ValidationException.cs
	namespace 	!
infobitservicebackend
 
.  
Application  +
.+ ,
Common, 2
.2 3

Exceptions3 =
;= >
public 
class 
ValidationException  
:! "
	Exception# ,
{ 
public 

IDictionary 
< 
string 
, 
string %
[% &
]& '
>' (
Errors) /
{0 1
get2 5
;5 6
}7 8
public 

ValidationException 
( 
)  
:! "
base# '
(' (
$str( X
)X Y
{ 
Errors		 
=		 
new		 

Dictionary		 
<		  
string		  &
,		& '
string		( .
[		. /
]		/ 0
>		0 1
(		1 2
)		2 3
;		3 4
}

 
public 

ValidationException 
( 
IEnumerable *
<* +
FluentValidation+ ;
.; <
Results< C
.C D
ValidationFailureD U
>U V
failuresW _
)_ `
: 	
this
 
( 
) 
{ 
Errors 
= 
failures 
. 
GroupBy 
( 
e 
=> 
e 
. 
PropertyName (
,( )
e* +
=>, .
e/ 0
.0 1
ErrorMessage1 =
)= >
. 
ToDictionary 
( 
group 
=>  "
group# (
.( )
Key) ,
,, -
group. 3
=>4 6
group7 <
.< =
ToArray= D
(D E
)E F
)F G
;G H
} 
} ˙
C:\Users\armad\Documentos\Estudio\Proyectos\infobitservicebackend\src\Application\Common\Exceptions\ForbiddenAccessException.cs
	namespace 	!
infobitservicebackend
 
.  
Application  +
.+ ,
Common, 2
.2 3

Exceptions3 =
;= >
public 
class $
ForbiddenAccessException %
:& '
	Exception( 1
{ 
public 
$
ForbiddenAccessException #
(# $
)$ %
:& '
base( ,
(, -
)- .
{/ 0
}1 2
} Ë
zC:\Users\armad\Documentos\Estudio\Proyectos\infobitservicebackend\src\Application\Common\Behaviours\ValidationBehaviour.cs
	namespace 	!
infobitservicebackend
 
.  
Application  +
.+ ,
Common, 2
.2 3

Behaviours3 =
;= >
public 
class 
ValidationBehaviour  
<  !
TRequest! )
,) *
	TResponse+ 4
>4 5
:6 7
IPipelineBehavior8 I
<I J
TRequestJ R
,R S
	TResponseT ]
>] ^
where 

TRequest 
: 
notnull 
{ 
private 
readonly 
IEnumerable  
<  !

IValidator! +
<+ ,
TRequest, 4
>4 5
>5 6
_validators7 B
;B C
public

 

ValidationBehaviour

 
(

 
IEnumerable

 *
<

* +

IValidator

+ 5
<

5 6
TRequest

6 >
>

> ?
>

? @

validators

A K
)

K L
{ 
_validators 
= 

validators  
;  !
} 
public 

async 
Task 
< 
	TResponse 
>  
Handle! '
(' (
TRequest( 0
request1 8
,8 9"
RequestHandlerDelegate: P
<P Q
	TResponseQ Z
>Z [
next\ `
,` a
CancellationTokenb s
cancellationToken	t Ö
)
Ö Ü
{ 
if 

( 
_validators 
. 
Any 
( 
) 
) 
{ 	
var 
context 
= 
new 
ValidationContext /
</ 0
TRequest0 8
>8 9
(9 :
request: A
)A B
;B C
var 
validationResults !
=" #
await$ )
Task* .
.. /
WhenAll/ 6
(6 7
_validators 
. 
Select "
(" #
v# $
=>% '
v 
. 
ValidateAsync #
(# $
context$ +
,+ ,
cancellationToken- >
)> ?
)? @
)@ A
;A B
var 
failures 
= 
validationResults ,
. 
Where 
( 
r 
=> 
r 
. 
Errors $
.$ %
Any% (
(( )
)) *
)* +
. 

SelectMany 
( 
r 
=>  
r! "
." #
Errors# )
)) *
. 
ToList 
( 
) 
; 
if 
( 
failures 
. 
Any 
( 
) 
) 
throw 
new 
ValidationException -
(- .
failures. 6
)6 7
;7 8
}   	
return!! 
await!! 
next!! 
(!! 
)!! 
;!! 
}"" 
}## ñ
ÇC:\Users\armad\Documentos\Estudio\Proyectos\infobitservicebackend\src\Application\Common\Behaviours\UnhandledExceptionBehaviour.cs
	namespace 	!
infobitservicebackend
 
.  
Application  +
.+ ,
Common, 2
.2 3

Behaviours3 =
;= >
public 
class '
UnhandledExceptionBehaviour (
<( )
TRequest) 1
,1 2
	TResponse3 <
>< =
:> ?
IPipelineBehavior@ Q
<Q R
TRequestR Z
,Z [
	TResponse\ e
>e f
whereg l
TRequestm u
:v w
notnullx 
{ 
private 
readonly 
ILogger 
< 
TRequest %
>% &
_logger' .
;. /
private		 
readonly		 

ITokenInfo		 

_tokenInfo		  *
;		* +
public 
'
UnhandledExceptionBehaviour &
(& '
ILogger' .
<. /
TRequest/ 7
>7 8
logger9 ?
,? @

ITokenInfoA K
	tokenInfoL U
)U V
{ 
_logger 
= 
logger 
; 

_tokenInfo 
= 
	tokenInfo 
; 
} 
public 

async 
Task 
< 
	TResponse 
>  
Handle! '
(' (
TRequest( 0
request1 8
,8 9"
RequestHandlerDelegate: P
<P Q
	TResponseQ Z
>Z [
next\ `
,` a
CancellationTokenb s
cancellationToken	t Ö
)
Ö Ü
{ 
try 
{ 	
return 
await 
next 
( 
) 
;  
} 	
catch 
( 
	Exception 
ex 
) 
{ 	
var 
requestName 
= 
typeof $
($ %
TRequest% -
)- .
.. /
Name/ 3
;3 4
var 
errorId 
= 
Guid 
. 
NewGuid &
(& '
)' (
.( )
ToString) 1
(1 2
)2 3
;3 4
var 
maskedToken 
= 

_tokenInfo (
.( )
RawToken) 1
?1 2
.2 3
Length3 9
>: ;
$num< >
? 
$" 
{ 

_tokenInfo 
.  
RawToken  (
.( )
	Substring) 2
(2 3
$num3 4
,4 5
$num6 7
)7 8
}8 9
$str9 <
{< =

_tokenInfo= G
.G H
RawTokenH P
[P Q
^Q R
$numR S
..S U
]U V
}V W
"W X
: 
$str 
; 
_logger   
.   
LogError   
(   
ex   
,    
$str	  ! ±
,
  ± ≤
errorId!! 
,!! 
requestName!! $
,!!$ %
maskedToken!!& 1
,!!1 2
request!!3 :
,!!: ;
ex!!< >
.!!> ?
Message!!? F
)!!F G
;!!G H
throw## 
;## 
}$$ 	
}%% 
}&& ©
{C:\Users\armad\Documentos\Estudio\Proyectos\infobitservicebackend\src\Application\Common\Behaviours\PerformanceBehaviour.cs
	namespace 	!
infobitservicebackend
 
.  
Application  +
.+ ,
Common, 2
.2 3

Behaviours3 =
;= >
public 
class  
PerformanceBehaviour !
<! "
TRequest" *
,* +
	TResponse, 5
>5 6
:7 8
IPipelineBehavior9 J
<J K
TRequestK S
,S T
	TResponseU ^
>^ _
where` e
TRequestf n
:o p
notnullq x
{ 
private		 
readonly		 
	Stopwatch		 
_timer		 %
;		% &
private

 
readonly

 
ILogger

 
<

 
TRequest

 %
>

% &
_logger

' .
;

. /
private 
readonly 

ITokenInfo 

_tokenInfo  *
;* +
public 
 
PerformanceBehaviour 
(  
ILogger  '
<' (
TRequest( 0
>0 1
logger2 8
,8 9

ITokenInfo: D
	tokenInfoE N
)N O
{ 
_timer 
= 
new 
	Stopwatch 
( 
)  
;  !
_logger 
= 
logger 
; 

_tokenInfo 
= 
	tokenInfo 
; 
} 
public 

async 
Task 
< 
	TResponse 
>  
Handle! '
(' (
TRequest( 0
request1 8
,8 9"
RequestHandlerDelegate: P
<P Q
	TResponseQ Z
>Z [
next\ `
,` a
CancellationTokenb s
cancellationToken	t Ö
)
Ö Ü
{ 
_timer 
. 
Start 
( 
) 
; 
var 
response 
= 
await 
next !
(! "
)" #
;# $
_timer 
. 
Stop 
( 
) 
; 
var 
elapsedMilliseconds 
=  !
_timer" (
.( )
ElapsedMilliseconds) <
;< =
var 
maskedToken 
= 

_tokenInfo $
.$ %
RawToken% -
?- .
.. /
Length/ 5
>6 7
$num8 :
?   
$"   
{   

_tokenInfo   
.   
RawToken   $
.  $ %
	Substring  % .
(  . /
$num  / 0
,  0 1
$num  2 3
)  3 4
}  4 5
$str  5 8
{  8 9

_tokenInfo  9 C
.  C D
RawToken  D L
[  L M
^  M N
$num  N O
..  O Q
]  Q R
}  R S
"  S T
:!! 
$str!! 
;!! 
if## 

(## 
elapsedMilliseconds## 
>##  !
$num##" %
)##% &
{$$ 	
var%% 
requestName%% 
=%% 
typeof%% $
(%%$ %
TRequest%%% -
)%%- .
.%%. /
Name%%/ 3
;%%3 4
_logger'' 
.'' 

LogWarning'' 
('' 
$str	'' ¨
,
''¨ ≠
requestName(( 
,(( 
maskedToken(( (
,((( )
elapsedMilliseconds((* =
,((= >
request((? F
)((F G
;((G H
})) 	
return++ 
response++ 
;++ 
},, 
}-- á
wC:\Users\armad\Documentos\Estudio\Proyectos\infobitservicebackend\src\Application\Common\Behaviours\LoggingBehaviour.cs
	namespace 	!
infobitservicebackend
 
.  
Application  +
.+ ,
Common, 2
.2 3

Behaviours3 =
;= >
public 
class 
LoggingBehaviour 
< 
TRequest &
>& '
:( ) 
IRequestPreProcessor* >
<> ?
TRequest? G
>G H
whereI N
TRequestO W
:X Y
notnullZ a
{ 
private		 
readonly		 
ILogger		 
_logger		 $
;		$ %
private

 
readonly

 

ITokenInfo

 

_tokenInfo

  *
;

* +
public 

LoggingBehaviour 
( 
ILogger #
<# $
TRequest$ ,
>, -
logger. 4
,4 5

ITokenInfo6 @
	tokenInfoA J
)J K
{ 
_logger 
= 
logger 
; 

_tokenInfo 
= 
	tokenInfo 
; 
} 
public 

Task 
Process 
( 
TRequest  
request! (
,( )
CancellationToken* ;
cancellationToken< M
)M N
{ 
var 
requestName 
= 
typeof  
(  !
TRequest! )
)) *
.* +
Name+ /
;/ 0
var 
maskedToken 
= 

_tokenInfo $
.$ %
RawToken% -
?- .
.. /
Length/ 5
>6 7
$num8 :
? 
$" 
{ 

_tokenInfo 
. 
RawToken $
.$ %
	Substring% .
(. /
$num/ 0
,0 1
$num2 3
)3 4
}4 5
$str5 8
{8 9

_tokenInfo9 C
.C D
RawTokenD L
[L M
^M N
$numN O
..O Q
]Q R
}R S
"S T
: 
$str 
; 
_logger 
. 
LogInformation 
( 
$str }
,} ~
requestName 
, 
maskedToken $
,$ %
request& -
)- .
;. /
return 
Task 
. 
CompletedTask !
;! "
} 
}   ç
}C:\Users\armad\Documentos\Estudio\Proyectos\infobitservicebackend\src\Application\Common\Behaviours\AuthorizationBehaviour.cs
	namespace 	!
infobitservicebackend
 
.  
Application  +
.+ ,
Common, 2
.2 3

Behaviours3 =
;= >
public 
class "
AuthorizationBehaviour #
<# $
TRequest$ ,
,, -
	TResponse. 7
>7 8
:9 :
IPipelineBehavior; L
<L M
TRequestM U
,U V
	TResponseW `
>` a
where 	
TRequest
 
: 
notnull 
{		 
private

 
readonly

 

ITokenInfo

 

_tokenInfo

  *
;

* +
public 
"
AuthorizationBehaviour !
(! "

ITokenInfo" ,
	tokenInfo- 6
)6 7
{ 

_tokenInfo 
= 
	tokenInfo 
; 
} 
public 

async 
Task 
< 
	TResponse 
>  
Handle! '
(' (
TRequest( 0
request1 8
,8 9"
RequestHandlerDelegate: P
<P Q
	TResponseQ Z
>Z [
next\ `
,` a
CancellationTokenb s
cancellationToken	t Ö
)
Ö Ü
{ 
var 
authorizeAttributes 
=  !
request" )
.) *
GetType* 1
(1 2
)2 3
.3 4
GetCustomAttributes4 G
<G H
AuthorizeAttributeH Z
>Z [
([ \
)\ ]
;] ^
if 

( 
authorizeAttributes 
.  
Any  #
(# $
)$ %
&&& (
!) *

_tokenInfo* 4
.4 5
HasValidToken5 B
)B C
{ 	
throw 
new '
UnauthorizedAccessException 1
(1 2
$str	2 É
)
É Ñ
;
Ñ Ö
} 	
return 
await 
next 
( 
) 
; 
} 
} 