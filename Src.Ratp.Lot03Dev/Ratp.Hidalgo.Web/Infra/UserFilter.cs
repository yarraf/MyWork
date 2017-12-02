using Core.Common.Encrypt;
using System;
using System.ServiceModel;
using System.Web.Mvc;

namespace Ratp.Hidalgo.Web.Infra
{
    public class UserFilter : System.Web.Mvc.ActionFilterAttribute
    {
        private readonly log4net.ILog Logger;

        public UserFilter()
        {
            this.Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string decryptResult = null;
            if (filterContext.HttpContext.Request.QueryString["USERID"] != null)
            {
                var marionV3proxy = new MarionProxy("MarionServicesSoap");
                try
                {
                     decryptResult = Encrypt.DecryptString(filterContext.HttpContext.Request.QueryString["USERID"]);
                }
                catch(Exception ex)
                {
                    this.Logger.Error(ex.Message);
                    filterContext.Result = new ViewResult { ViewName = "Error" };
                }
               
                int outParam = -1;
                if (int.TryParse(decryptResult, out outParam))
                {
                    try
                    {
                        var resultOfService = marionV3proxy.IsUserAuthorized(outParam);
                        if (resultOfService != null)
                        {
                            this.Logger.Info(string.Format("Accès autorisé pour l'utilisateur {0}", resultOfService.Login));
                            filterContext.Controller.ViewBag.USERID = resultOfService.Login;
                            filterContext.Controller.TempData["USERID"] = outParam;

                        }
                        else
                        {
                            filterContext.Result = new ViewResult { ViewName = "Error" };
                        }
                    }
                    catch (EndpointNotFoundException endpointException)
                    {
                        this.Logger.Error(endpointException.Message);
                        filterContext.Result = new ViewResult { ViewName = "Error" };
                    }

                    catch (Exception ex)
                    {
                        this.Logger.Error("Accès non autorisé, la clé de chiffrement est incorrect.");
                        filterContext.Result = new ViewResult { ViewName = "Error" };
                    }
                }
                else
                {
                    this.Logger.Error("Accès non autorisé, veuillez vérifier les droits de l'utilisateur, ou utilisateur n'existe pas dans la base de données.");
                    filterContext.Result = new ViewResult { ViewName = "Error" };
                }
            }

            else if (filterContext.Controller.TempData["USERID"] != null)
            {
                var marionV3proxy = new MarionProxy("MarionServicesSoap");
                int outParam = -1;
                if (int.TryParse(filterContext.Controller.TempData["USERID"].ToString(), out outParam))
                {
                    var resultOfService = marionV3proxy.IsUserAuthorized(outParam);
                    if (resultOfService != null)
                    {
                        filterContext.Controller.ViewBag.USERID = resultOfService.Login;
                        filterContext.Controller.TempData.Keep("USERID");
                    }
                }
                else
                {
                    this.Logger.Error("Accès non autorisé, veuillez vérifier les droits de l'utilisateur, ou utilisateur n'existe pas dans la base de données.");
                    filterContext.Result = new ViewResult { ViewName = "Error" };
                }
            }
            else
            {
                this.Logger.Error("Accès non autorisé, veuillez vérifier les droits de l'utilisateur, ou utilisateur n'existe pas dans la base de données.");
                filterContext.Result = new ViewResult { ViewName = "Error" };
            }
        }
    }
}