﻿select
	month
	,year
	,rtrim(ltrim(substring(agent, len(agent) - charindex('-', reverse(agent)) + 2, 50))) as devManuf
	,rtrim(ltrim(substring(agent, 1, len(agent) - charindex('-', reverse(agent))))) as system
	,subDel
	,sum(qtyOfTSInTI) as qtyOfTSInTI
	,count(*) as qtyDefect
	,round(convert(float, sum(qtyOfTSInTI)) / (case when count(*) <> 0 then count(*) else 1 end) * 100, 2) as percOfTSInTI
from
	(
		select
			substring(dt_final,4,2) as month
			,substring(dt_final,7,2) as year
			,replace(encaminhado_para,'–', '-') as agent
			,convert(varchar, cast(substring(subprojeto,4,8) as int)) + ' ' + convert(varchar,cast(substring(entrega,8,8) as int)) as subDel
			,case when Erro_Detectavel_Em_Desenvolvimento = 'SIM' then 1 else 0 end as qtyOfTSInTI
		from 
			alm_defeitos WITH (NOLOCK)
		where
			status_atual = 'CLOSED'
			and dt_final <> ''
			and ciclo in ('TI', 'UAT')
			and origem like '%Construção%'
			and subprojeto = '@subproject'
			and entrega = '@delivery'
	) a1
group by
	month
	,year
	,rtrim(ltrim(substring(agent, len(agent) - charindex('-', reverse(agent)) + 2, 50)))
	,rtrim(ltrim(substring(agent, 1, len(agent) - charindex('-', reverse(agent)))))
	,subDel
order by
	2, 1, 3, 4
