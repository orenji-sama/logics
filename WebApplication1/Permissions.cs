﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Data;


namespace ASGuf2.DAL
{
    public class Permissions
    {
        public class Auth
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public OracleCommand OraQueryProcFunc(OracleConnection conn, string FuncName,
            OracleParameter[] p, OracleTransaction trans)
        {
            OracleCommand ComOra = new OracleCommand(FuncName, conn);
            ComOra.Transaction = trans;
            ComOra.CommandType = CommandType.StoredProcedure;
            ComOra.Connection = conn;
            ComOra.Parameters.AddRange(p);
            ComOra.ExecuteNonQuery();
            return ComOra;
        }

        public DataTable GetCursor(OracleConnection conn, OracleTransaction tran, string query)
        {
            OracleCommand ComOra = new OracleCommand(query, conn);
            ComOra.Transaction = tran;
            ComOra.Connection = conn;
            OracleDataReader reader = ComOra.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(reader, LoadOption.PreserveChanges);
            return dt;
        }

        public string OraQuery(OracleConnection conn, OracleTransaction tran, string Command)
        {
            OracleCommand ComOra = new OracleCommand(Command, conn, tran);
            object result = ComOra.ExecuteScalar();
            return !result.Equals(DBNull.Value) ? (string)result : "";
        }

        public int GetIDRubrBySPDproc(OracleConnection conn, OracleTransaction tran, int id_rubr_)
        {
            OracleParameter[] p = new OracleParameter[1 + 1];
            p[0] = new OracleParameter("RETURN_VALUE", OracleType.Number); p[0].Direction = ParameterDirection.ReturnValue;
            p[1] = new OracleParameter("id_rubr_", OracleType.VarChar); p[1].Value = id_rubr_; p[1].Direction = ParameterDirection.Input;

            OraQueryProcFunc(conn, "asguf2.admin_pkg.GetIDRubrBySPDproc", p, tran);
            return Convert.ToInt32(p[0].Value);
        }

        public string GetLoginByFullName(OracleConnection conn, OracleTransaction tran, string FullName)
        {
            OracleParameter[] p = new OracleParameter[1 + 1];
            p[0] = new OracleParameter("RETURN_VALUE", OracleType.VarChar); p[0].Size = 2048; p[0].Direction = ParameterDirection.ReturnValue;
            p[1] = new OracleParameter("FullName", OracleType.VarChar); p[1].Value = FullName; p[1].Direction = ParameterDirection.Input;

            OraQueryProcFunc(conn, "asguf2.permissions_pkg.GetLoginByFullName", p, tran);
            return Convert.ToString(p[0].Value);
        }

        public bool CheckGroup(OracleConnection conn, OracleTransaction tran, string ActLogin, string GroupName)
        {
            if (ActLogin == null)
                ActLogin = "";
            OracleParameter[] p = new OracleParameter[1 + 2];
            p[0] = new OracleParameter("RETURN_VALUE", OracleType.Number); p[0].Direction = ParameterDirection.ReturnValue;
            p[1] = new OracleParameter("ActLogin", OracleType.VarChar); p[1].Value = ActLogin; p[1].Direction = ParameterDirection.Input;
            p[2] = new OracleParameter("GroupName", OracleType.VarChar); p[2].Value = GroupName; p[2].Direction = ParameterDirection.Input;

            OraQueryProcFunc(conn, "asguf2.permissions_pkg.CheckGroup", p, tran);

            return Convert.ToBoolean(Convert.ToInt32(p[0].Value));
        }

        public bool CheckGroup(OracleConnection conn, OracleTransaction tran, string ActLogin)
        {
            OracleParameter[] p = new OracleParameter[1 + 1];
            p[0] = new OracleParameter("RETURN_VALUE", OracleType.Number); p[0].Direction = ParameterDirection.ReturnValue;
            p[1] = new OracleParameter("ActLogin", OracleType.VarChar); p[1].Value = ActLogin; p[1].Direction = ParameterDirection.Input;

            OraQueryProcFunc(conn, "asguf2.permissions_pkg.CheckGroup", p, tran);

            return Convert.ToBoolean(Convert.ToInt32(p[0].Value));
        }
    }
}