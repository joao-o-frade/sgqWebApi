declare @t table (
	subproject varchar(11),
	delivery varchar(15)
)
insert into @t 
select distinct
	subproject,
	delivery
from
	(
		select distinct
			cts.Subprojeto as subproject
			,cts.Entrega as delivery
		from
			ALM_CTs cts with (NOLOCK) 
	) aux1
	inner join ALM_Projetos as alm_p with (NOLOCK) 
		on alm_p.Subprojeto = aux1.subproject and
			alm_p.Entrega = aux1.delivery and
			alm_p.Ativo = 'Y'

	inner join biti_subprojetos biti_s WITH (NOLOCK)
	on biti_s.id = aux1.subproject and
		biti_s.estado <> 'CANCELADO'

select 
    sgq_p.id
    ,t.subproject as subproject
    ,t.delivery as delivery
    ,convert(varchar, cast(substring(t.subproject,4,8) as int)) + ' ' + convert(varchar,cast(substring(t.delivery,8,8) as int)) as subDel
    ,biti_s.nome as name
    ,biti_s.objetivo as objective
    ,biti_s.classificacao_nome as classification
	,replace(replace(replace(replace(replace(biti_s.estado,'CONSOLIDA플O E APROVA플O DO PLANEJAMENTO','CONS/APROV. PLAN'),'PLANEJAMENTO','PLANEJ.'),'DESENHO DA SOLU플O','DES.SOL'),'VALIDA플O','VALID.'),'AGUARDANDO','AGUAR.') as state
	
	,sgq_p.testState
	,sgq_p.deployOff
	,sgq_p.lossRelease
	,sgq_p.lossReleaseReason
	
    ,(select Sigla from sgq_meses m where m.id = sgq_p.currentReleaseMonth) + ' ' + convert(varchar, sgq_p.currentReleaseYear) as currentRelease
    ,(select Sigla from sgq_meses m where m.id = sgq_p.currentReleaseMonth) + ' ' + convert(varchar, sgq_p.currentReleaseYear) as clarityRelease
	,currentReleaseMonth
	,currentReleaseYear
	,clarityReleaseMonth
	,clarityReleaseYear

    ,biti_s.gerente_projeto as GP
	,(select top 1 gestor_n4 from biti_Usuarios where nome = biti_s.gerente_projeto) as GP_N4
	,(select top 1 gestor_n3 from biti_Usuarios where nome = biti_s.gerente_projeto) as GP_N3

	,biti_s.Lider_Tecnico as LT
    ,biti_s.Gestor_Direto_LT as LT_N4
    ,biti_s.Gestor_Do_Gestor_LT as LT_N3

    ,biti_s.UN as UN
    ,sgq_p.trafficLight as trafficLight
    ,sgq_p.rootCause as rootCause
    ,sgq_p.actionPlan as actionPlan
    ,sgq_p.informative as informative
    ,sgq_p.attentionPoints as attentionPoints
    ,sgq_p.attentionPointsIndicators as attentionPointsOfIndicators
    ,sgq_p.iterationsActive
    ,sgq_p.iterationsSelected
from 
	@t t
    inner join sgq_projects sgq_p
		on sgq_p.subproject = t.subproject and
		   sgq_p.delivery = t.delivery

    inner join alm_projetos alm_p WITH (NOLOCK)
		on alm_p.subprojeto = t.subproject and
			alm_p.entrega = t.delivery

    inner join biti_subprojetos biti_s WITH (NOLOCK)
		on biti_s.id = t.subproject
where 
	sgq_p.currentReleaseYear is not null
order by
    t.subproject,
    t.delivery

--select
--	id,
--	subproject,
--	delivery,
--	project,
--	name,
--	objective,
--	classification,
--	state,
--	release,
--	GP,
--	N3,
--	UN,
--	trafficLight,
--	rootCause,
--	actionPlan,
--	informative,
--	attentionPoints,
--	attentionPointsOfIndicators,
--	IterationsActive,
--	IterationsSelected,

--	(select count(*) 
--	from ALM_CTs WITH (NOLOCK)
--	where 
--		subprojeto = aux.subproject and
--		entrega = aux.delivery and
--		Status_Exec_CT <> 'CANCELLED'
--	) as total,

--	(select count(*) 
--	from ALM_CTs WITH (NOLOCK)
--	where 
--		subprojeto = aux.subproject and
--		entrega = aux.delivery and
--		Status_Exec_CT <> 'CANCELLED' and
--		substring(dt_planejamento,7,2) + substring(dt_planejamento,4,2) + substring(dt_planejamento,1,2) <= convert(varchar(6), getdate(), 12) and
--		dt_planejamento <> ''
--	) as planned,

--	(select count(*)
--	from ALM_CTs WITH (NOLOCK)
--	where 
--		subprojeto = aux.subproject and
--		entrega = aux.delivery and
--		Status_Exec_CT = 'PASSED' and 
--		dt_execucao <> ''
--	) as realized,

--	(select 
--		(case when sum(planned) - sum(realized) >= 0 then sum(planned) - sum(realized) else 0 end) as GAP
--	from
--		(
--		select 
--			substring(dt_planejamento,4,5) as date, 
--			1 as planned,
--			0 as realized
--		from ALM_CTs WITH (NOLOCK)
--		where 
--			subprojeto = aux.subproject and
--			entrega = aux.delivery and
--			Status_Exec_CT <> 'CANCELLED' and
--			substring(dt_planejamento,7,2) + substring(dt_planejamento,4,2) + substring(dt_planejamento,1,2) <= convert(varchar(6), getdate(), 12) and
--			dt_planejamento <> ''

--		union all

--		select 
--			substring(dt_execucao,4,5) as date, 
--			0 as planned,
--			1 as realized
--		from ALM_CTs WITH (NOLOCK)
--		where 
--			subprojeto = aux.subproject and
--			entrega = aux.delivery and
--			Status_Exec_CT = 'PASSED' and 
--			dt_execucao <> ''
--		) Aux
--	) as gap
--from
--	(
--    select 
--        sgq_p.id,
--        sgq_p.subproject as subproject,
--        sgq_p.delivery as delivery,
--        convert(varchar, cast(substring(sgq_p.subproject,4,8) as int)) + ' ' + convert(varchar,cast(substring(sgq_p.delivery,8,8) as int)) as subDel,
--        biti_s.nome as name,
--        biti_s.objetivo as objective,
--        biti_s.classificacao_nome as classification,
--        replace(replace(replace(replace(replace(biti_s.estado,'CONSOLIDA플O E APROVA플O DO PLANEJAMENTO','CONS/APROV. PLAN'),'PLANEJAMENTO','PLANEJ.'),'DESENHO DA SOLU플O','DES.SOL'),'VALIDA플O','VALID.'),'AGUARDANDO','AGUAR.') as state,
--        (select Sigla from sgq_meses m where m.id = sgq_p.currentReleaseMonth) + ' ' + convert(varchar, sgq_p.currentReleaseYear) as release,
--        biti_s.Gerente_Projeto as GP,
--        biti_s.Gestor_Do_Gestor_LT as N3,
--        biti_s.UN as UN,
--        sgq_p.trafficLight as trafficLight,
--        sgq_p.rootCause as rootCause,
--        sgq_p.actionPlan as actionPlan,
--        sgq_p.informative as informative,
--        sgq_p.attentionPoints as attentionPoints,
--        sgq_p.attentionPointsIndicators as attentionPointsOfIndicators,
--        sgq_p.IterationsActive,
--        sgq_p.IterationsSelected
--    from 
--        sgq_projects sgq_p
--        inner join alm_projetos alm_p WITH (NOLOCK)
--        on alm_p.subprojeto = sgq_p.subproject and
--            alm_p.entrega = sgq_p.delivery and
--            alm_p.ativo = 'Y'

--        inner join biti_subprojetos biti_s WITH (NOLOCK)
--        on biti_s.id = sgq_p.subproject and
--		    biti_s.estado <> 'CANCELADO'
--    where 
--		sgq_p.currentReleaseYear is not null
--	) aux
--order by
--    subproject,
--    delivery
