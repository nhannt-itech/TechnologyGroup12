using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using TechnologyGroup12.Models.ExtentionModels;
using TechnologyGroup12.Models.ExtentionModels.IExtensionModels;

namespace TechnologyGroup12.Controllers
{
    public class ServerController : Controller
    {
        private readonly ConnectionStrings _connectionStrings;
        private readonly IWritableOptions<ConnectionStrings> _writableCnt;

        public ServerController(IOptionsSnapshot<ConnectionStrings> connectionStrings, IWritableOptions<ConnectionStrings> writableCnt)
        {
            _connectionStrings = connectionStrings.Value;
            _writableCnt = writableCnt;
        }

        public IActionResult Index()
        {
            //_writableCnt.Update(opt =>
            //{
            //    opt.DefaultConnection = "Server=localhost\\SQLEXPRESS;Database=TechnologyStoreDB;Trusted_Connection=True;";
            //});
            List<string> lDatabase = new List<string>();
            ServerConnection serverConnection = new ServerConnection();
            serverConnection.serverName = @"";
            serverConnection.connectionString = _writableCnt.Value.DefaultConnection.ToString();

            serverConnection.databaseTable = lDatabase.Select(i => new SelectListItem
            {
                Text = i,
                Value = i
            });

            return View(serverConnection);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(ServerConnection serverConnection)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var connectionString = ExecuteConnection.Connect(serverConnection.serverName, serverConnection.databaseName, serverConnection.userName,
                        serverConnection.passWord);
                    if (serverConnection.userName != null)
                    {
                        connectionString = ExecuteConnection.Connect(serverConnection.serverName, serverConnection.databaseName, serverConnection.userName,
                        serverConnection.passWord);
                    }
                    else
                    {
                        connectionString = ExecuteConnection.Connect(serverConnection.serverName, serverConnection.databaseName);
                    }


                    List<string> lDatabase = new List<string>();
                    connectionString.Open();
                    SqlCommand cmd = new SqlCommand("SELECT name from sys.databases", connectionString);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lDatabase.Add(dr[0].ToString());
                        }
                    }

                    serverConnection.databaseTable = lDatabase.Select(i => new SelectListItem
                    {
                        Text = i,
                        Value = i
                    });

                    if (serverConnection.userName != null)
                    {
                        _writableCnt.Update(opt =>
                        {
                            opt.DefaultConnection = @"Server=" + serverConnection.serverName +
                            @";Database=" + serverConnection.databaseName +
                            @";User Id=" + serverConnection.userName +
                            @"; Password=" + serverConnection.passWord +
                            @";";//;Trusted_Connection=True
                        });
                    }
                    else
                    {
                        _writableCnt.Update(opt =>
                        {
                            opt.DefaultConnection = @"Server=" + serverConnection.serverName +
                            @";Database=" + serverConnection.databaseName +
                            @";Trusted_Connection=True;MultipleActiveResultSets=true";
                        });
                    }

                    

                    return View(serverConnection);
                    //"Server =localhost\\SQLEXPRESS;Database=TechnologyGroup12DB;Trusted_Connection=True;MultipleActiveResultSets=true"


                }
                else if (serverConnection.serverName != null)
                {
                    try
                    {
                        var connectionString = ExecuteConnection.Connect(serverConnection.serverName, serverConnection.userName,
                        serverConnection.passWord);
                        if (serverConnection.userName != null)
                        {
                            connectionString = ExecuteConnection.Connect(serverConnection.serverName, serverConnection.userName,
                        serverConnection.passWord);
                        }
                        else
                        {
                            connectionString = ExecuteConnection.Connect(serverConnection.serverName);
                        }

                        List<string> lDatabase = new List<string>();
                        connectionString.Open();
                        SqlCommand cmd = new SqlCommand("SELECT name from sys.databases", connectionString);
                        using (IDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                lDatabase.Add(dr[0].ToString());
                            }
                        }

                        serverConnection.databaseTable = lDatabase.Select(i => new SelectListItem
                        {
                            Text = i,
                            Value = i
                        });

                        return View(serverConnection);
                    }
                    catch
                    {
                        serverConnection.serverName = "ErrorServer";
                        List<string> lDatabase = new List<string>();

                        serverConnection.databaseTable = lDatabase.Select(i => new SelectListItem
                        {
                            Text = i,
                            Value = i
                        });
                        return View(serverConnection);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                 List<string> lDatabase = new List<string>();

                    serverConnection.databaseTable = lDatabase.Select(i => new SelectListItem
                    {
                        Text = i,
                        Value = i
                    });
                return View(serverConnection);
            }
            return View(serverConnection);
        }
    }
}

