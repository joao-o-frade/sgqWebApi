﻿select
	substring(date,4,2) as month
	,substring(date,7,2) as year
	,testManuf
	,system
	,subDel
	,count(*) as qtyDefect
	,sum(qtyDefectUAT) as qtyDefectUAT
from
	(
		select
			case when dt_inicial <> '' 
				then dt_inicial
				else 
					case when dt_final <> '' 
						then dt_final
						else dt_ultimo_status
					end
			end as date

			,(case when IsNull(fabrica_teste,'') <> '' then fabrica_teste else 'N/A' end) as testManuf
			,sistema_ct as system
			,convert(varchar, cast(substring(subprojeto,4,8) as int)) + ' ' + convert(varchar,cast(substring(entrega,8,8) as int)) as subDel
			,case when ciclo = 'UAT' then 1 else 0 end qtyDefectUAT
		from 
			alm_defeitos WITH (NOLOCK)
		where
			ciclo in ('TI', 'UAT')
			and status_atual <> 'CANCELLED'
			and subprojeto = '@subproject'
			and entrega = '@delivery'
	) aux1
group by 
	substring(date,4,2)
	,substring(date,7,2)
	,testManuf
	,system
	,subDel
order by
	2, 1, 3, 4