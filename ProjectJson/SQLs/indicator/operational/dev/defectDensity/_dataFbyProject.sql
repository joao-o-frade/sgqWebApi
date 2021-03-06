﻿declare @cts table (
	devManuf varchar(50), 
	system varchar(50),
	subDel varchar(26),
	ct int,
	yearMonth varchar(4)
)
insert into @cts 
select 
	(case when IsNull(ct.fabrica_desenvolvimento,'') <> '' then fabrica_desenvolvimento else 'N/A' end) as devManuf
	,(case when IsNull(ct.sistema,'') <> '' then ct.sistema else 'N/A' end) as system
	,ct.subprojeto + ct.entrega as subDel
	,ct.ct
	,(
		select
			min(substring(ex.dt_execucao,7,2) + substring(ex.dt_execucao,4,2))
		from 
			alm_execucoes ex WITH (NOLOCK)
		where
			ex.subprojeto = ct.subprojeto
			and ex.entrega = ct.entrega
			and ex.ct = ct.ct
			and ex.status not in ('CANCELLED', 'NO RUN')
			and ex.dt_execucao <> ''
	) as yearMonth
from 
	ALM_CTs ct WITH (NOLOCK)
where
	ct.Massa_Teste <> 'SIM'
	and ct.Status_Exec_CT not in ('CANCELLED', 'NO RUN')
	and ct.Ciclo in ('TI', 'UAT')
	and ct.subprojeto = '@subproject'
	and ct.entrega = '@delivery'


declare @dfs table (
	subDel varchar(26),
	ct int,
	qtyDefect int
)
insert into @dfs
select 
	subprojeto + entrega as subDel
	,ct
	,count(*) as qtyDefect
from alm_defeitos df
where 
	df.status_atual = 'CLOSED'
	and df.Origem like '%CONSTRUÇÃO%'
	and df.Ciclo in ('TI', 'UAT')
	and df.subprojeto = '@subproject'
	and df.entrega = '@delivery'
group by
	subprojeto
	,entrega
	,ct


select
	month
	,year
    ,devManuf
	,system
	,convert(varchar, cast(substring(subDel,4,8) as int)) + ' ' + convert(varchar,cast(substring(subDel,19,8) as int)) as subDel
    ,sum(qtyDefect) as qtyDefect
    ,count(*) as qtyCt
    ,round(convert(float, sum(qtyDefect)) / (case when count(*) = 0 then 1 else count(*) end) * 100,2) as density
from
	(
		select 
			substring(cts.yearMonth, 3, 2) as month
			,substring(cts.yearMonth, 1, 2) as year
			,cts.devManuf
			,cts.system
			,cts.subDel
			,isnull(dfs.qtyDefect,0) as qtyDefect
		from
			@cts cts
			left join @dfs dfs
				on
					dfs.subDel = cts.subDel
					and dfs.ct = cts.ct
	) aux1
group by
	month,
	year,
    devManuf,
	system,
	subDel
order by
	year,
	month
