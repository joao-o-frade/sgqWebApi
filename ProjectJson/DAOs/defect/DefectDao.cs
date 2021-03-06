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
	public class DefectDao
    {
		private Connection connection;

		public DefectDao()
		{
			connection = new Connection(Bancos.Sgq);
		}

		public void Dispose()
		{
			connection.Dispose();
		}

        public IList<Defect> DefectsOpen(SubprojectDelivery subprojectDelivery) {
            string sql = File.ReadAllText(HttpContext.Current.Server.MapPath(@"~\sqls\defect\defectsOpen.sql"), Encoding.Default);
            sql = sql.Replace("@subproject", subprojectDelivery.subproject);
            sql = sql.Replace("@delivery", subprojectDelivery.delivery);
            var result = connection.Executar<Defect>(sql);
            return result;
        }

        public DefectDetail DefectDetail(Defect defect) {
            string sql = File.ReadAllText(HttpContext.Current.Server.MapPath(@"~\sqls\defect\defectDetail.sql"), Encoding.Default);
            sql = sql.Replace("@subproject", defect.subproject);
            sql = sql.Replace("@delivery", defect.delivery);
            sql = sql.Replace("@defect", defect.id.ToString());
            var result = connection.Executar<DefectDetail>(sql);
            // List<defectDetail> result = connection.Executar<defectDetail>(sql);
            return result[0];
        }

        public IList<DefectTime> DefectTime(Defect defect) {
            string sql = File.ReadAllText(HttpContext.Current.Server.MapPath(@"~\sqls\defect\defectTime.sql"), Encoding.Default);
            sql = sql.Replace("@subproject", defect.subproject);
            sql = sql.Replace("@delivery", defect.delivery);
            sql = sql.Replace("@defect", defect.id.ToString());
            var result = connection.Executar<DefectTime>(sql);
            return result;
        }
        
        //public IList<DefectsOpen> DefectsOpenInDevManuf(string subproject, string delivery)
        //{
        //    string sql = File.ReadAllText(HttpContext.Current.Server.MapPath(@"~\sqls\defect\defectsOpenInDevManuf.sql"), Encoding.Default);
        //    sql = sql.Replace("@subproject", subproject);
        //    sql = sql.Replace("@delivery", delivery);
        //    // List<DefectsOpen> result = connection.Executar<DefectsOpen>(sql);
        //    var result = connection.Executar<DefectsOpen>(sql);
        //    return result;
        //}

        //public IList<DefectsOpen> DefectsOpenInTestManuf(string subproject, string delivery)
        //{
        //    string sql = File.ReadAllText(HttpContext.Current.Server.MapPath(@"~\sqls\defect\defectsOpenInTestManuf.sql"), Encoding.Default);
        //    sql = sql.Replace("@subproject", subproject);
        //    sql = sql.Replace("@delivery", delivery);
        //    var result = connection.Executar<DefectsOpen>(sql);
        //    return result;
        //}

        public IList<DefectsOpen> DefectsOpenInDevManufIterations(string subproject, string delivery, List<string> iterations)
        {
            string sql = File.ReadAllText(HttpContext.Current.Server.MapPath(@"~\sqls\defect\defectsOpenInDevManufIteration.sql"), Encoding.Default);
            sql = sql.Replace("@subproject", subproject);
            sql = sql.Replace("@delivery", delivery);
            sql = sql.Replace("@iterations", "'" + string.Join("','", iterations.ToArray()) + "'");
            var result = connection.Executar<DefectsOpen>(sql);
            return result;
        }

        public IList<DefectsOpen> DefectsOpenInTestManufIterations(string subproject, string delivery, List<string> iterations)
        {
            string sql = File.ReadAllText(HttpContext.Current.Server.MapPath(@"~\sqls\defect\defectsOpenInTestManufIteration.sql"), Encoding.Default);
            sql = sql.Replace("@subproject", subproject);
            sql = sql.Replace("@delivery", delivery);
            sql = sql.Replace("@iterations", "'" + string.Join("','", iterations.ToArray()) + "'");
            var result = connection.Executar<DefectsOpen>(sql);
            return result;
        }


        public IList<DefectStatus> defectStatusFbyProject(string subproject, string delivery)
        {
            string sql = File.ReadAllText(HttpContext.Current.Server.MapPath(@"~\sqls\defect\defectStatusFbyProject.sql"), Encoding.Default);
            sql = sql.Replace("@subproject", subproject);
            sql = sql.Replace("@delivery", delivery);
            var result = connection.Executar<DefectStatus>(sql);
            return result;
        }

        public IList<DefectStatus> DefectStatusByProjectIterations(string subproject, string delivery, List<string> iterations)
        {
            string sql = File.ReadAllText(HttpContext.Current.Server.MapPath(@"~\sqls\defect\defectStatusFbyProjectIteration.sql"), Encoding.Default);
            sql = sql.Replace("@subproject", subproject);
            sql = sql.Replace("@delivery", delivery);
            sql = sql.Replace("@iterations", "'" + string.Join("','", iterations.ToArray()) + "'");
            var result = connection.Executar<DefectStatus>(sql);
            return result;
        }


        public IList<DefectStatus> defectGroupOrigin(string subproject, string delivery)
        {
            string sql = File.ReadAllText(HttpContext.Current.Server.MapPath(@"~\sqls\defect\defectGroupOrigin.sql"), Encoding.Default);
            sql = sql.Replace("@subproject", subproject);
            sql = sql.Replace("@delivery", delivery);
            List<DefectStatus> result = connection.Executar<DefectStatus>(sql);
            return result;
        }

        public IList<DefectStatus> defectGroupOriginIterations(string subproject, string delivery, List<string> iterations)
        {
            string sql = File.ReadAllText(HttpContext.Current.Server.MapPath(@"~\sqls\defect\defectGroupOriginIteration.sql"), Encoding.Default);
            sql = sql.Replace("@subproject", subproject);
            sql = sql.Replace("@delivery", delivery);
            sql = sql.Replace("@iterations", "'" + string.Join("','", iterations.ToArray()) + "'");
            List<DefectStatus> result = connection.Executar<DefectStatus>(sql);
            return result;
        }


    }
}
