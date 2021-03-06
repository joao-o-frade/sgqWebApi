using Classes;
using ProjectWebApi.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web;

namespace ProjectWebApi.Daos
{
    public class InformationalReleaseDao {
        private Connection connection;

        public InformationalReleaseDao()
        {
            connection = new Connection(Bancos.Sgq);
        }

        public void Dispose()
        {
            connection.Dispose();
        }

        public IList<Grouper> LoadData()
        {
            string sql = File.ReadAllText(HttpContext.Current.Server.MapPath(@"~\sqls\informational\release\loadData.sql"), Encoding.Default);
            return connection.Executar<Grouper>(sql);
        }

        public Grouper get(string id)
        {
            string sql = @"
            select 
	            id, 
	            name, 
                type,
	            executiveSummary,
	            case 
		            when 'VERMELHO' in (
			            select p.trafficLight
			            from SGQ_ProjectsXGroupers pg
				            left join sgq_projects p
				              on p.id = pg.project
			            where pg.Grouper = g.id
		            ) then 'VERMELHO'

		            when 'AMARELO' in (
			            select p.trafficLight
			            from SGQ_ProjectsXGroupers pg
				            left join sgq_projects p
				              on p.id = pg.project
			            where pg.Grouper = g.id
		            ) then 'AMARELO'

		            when 'VERDE' in (
			            select p.trafficLight
			            from SGQ_ProjectsXGroupers pg
				            left join sgq_projects p
				              on p.id = pg.project
			            where pg.Grouper = g.id
		            ) then 'VERDE'
	            end as trafficLight
            from	 
	            SGQ_Groupers g
            where 
                id = @id
            order by 
	            name
            ";
            sql = sql.Replace("@id", id);

            var list = connection.Executar<Grouper>(sql);

            return list[0];
        }

        public Grouper getByName(string name)
        {
            string sql = @"
            select 
	            id, 
	            name, 
                type,
	            executiveSummary,
	            case 
		            when 'VERMELHO' in (
			            select p.trafficLight
			            from SGQ_ProjectsXGroupers pg
				            left join sgq_projects p
				              on p.id = pg.project
			            where pg.Grouper = g.id
		            ) then 'VERMELHO'

		            when 'AMARELO' in (
			            select p.trafficLight
			            from SGQ_ProjectsXGroupers pg
				            left join sgq_projects p
				              on p.id = pg.project
			            where pg.Grouper = g.id
		            ) then 'AMARELO'

		            when 'VERDE' in (
			            select p.trafficLight
			            from SGQ_ProjectsXGroupers pg
				            left join sgq_projects p
				              on p.id = pg.project
			            where pg.Grouper = g.id
		            ) then 'VERDE'
	            end as trafficLight
            from	 
	            SGQ_Groupers g
            where 
                name = '@name'
            order by 
	            name
            ";
            sql = sql.Replace("@name", name);

            var list = connection.Executar<Grouper>(sql);

            return list[0];
        }

        public Grouper Create(Grouper item)
        {
            var connection = new Connection(Bancos.Sgq);

            if (item.name == null)
                item.name = "";

            if (item.executiveSummary == null)
                item.executiveSummary = "";

            bool resultado = false;
            if (item == null) throw new ArgumentNullException("item");
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection.connection;
                command.CommandText = @"
                    insert into SGQ_Groupers
                        (name, type, executiveSummary) 
                    values 
                        (@name, @type, @executiveSummary)
                ";
                command.Parameters.AddWithValue("name", item.name);
                command.Parameters.AddWithValue("type", item.type);
                command.Parameters.AddWithValue("executiveSummary", item.executiveSummary);

                int i = command.ExecuteNonQuery();
                resultado = i > 0;
            }

            var createItem = this.getByName(item.name);
            connection.Dispose();
            return createItem;
        }

        public void Update(string id, Grouper item)
        {
            var connection = new Connection(Bancos.Sgq);

            if (item.name == null)
                item.name = "";

            if (item.executiveSummary == null)
                item.executiveSummary = "";

            bool resultado = false;
            if (item == null) throw new ArgumentNullException("item");
            if (id == "0") throw new ArgumentNullException("id");
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection.connection;
                command.CommandText = @"
                    update SGQ_Groupers
                    set
                        name = @name,
                        type = @type,
                        executiveSummary = @executiveSummary
                    where
                        id = @id";
                command.Parameters.AddWithValue("id", item.id);
                command.Parameters.AddWithValue("name", item.name);
                command.Parameters.AddWithValue("type", item.type);
                command.Parameters.AddWithValue("executiveSummary", item.executiveSummary);

                int i = command.ExecuteNonQuery();
                resultado = i > 0;
            }
            connection.Dispose();
        }

        public void Delete(int id)
        {
            var connection = new Connection(Bancos.Sgq);

            bool resultado = false;
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection.connection;
                command.CommandText = @"
                    delete SGQ_ProjectsXGroupers where Grouper = @id;
                    delete SGQ_Groupers where id = @id;
                ";
                command.Parameters.AddWithValue("id", id);

                int i = command.ExecuteNonQuery();
                resultado = i > 0;
            }
            connection.Dispose();
        }
    }
}