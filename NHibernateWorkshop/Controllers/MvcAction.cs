using System;
using System.Web.Mvc;
using NHibernateWorkshop.Data;
using Ninject;

namespace NHibernateWorkshop.Controllers
{
    public abstract class MvcAction : Controller
    {
        [Inject]
        public SessionManager SessionManager { get; set; }
        protected EntityStore Db { get { return SessionManager.GetEntityStore(); } }

        protected MvcAction()
        {
            this.ActionInvoker = new HttpMethodActionInvoker(this.GetType());
        }

        protected RedirectToRouteResult RedirectTo<TAction>(object routeValues = null) where TAction : MvcAction
        {
            var info = new MvcActionInfo(typeof(TAction));
            return RedirectToAction(info.Action, info.Controller, routeValues);
        }

        private class HttpMethodActionInvoker : ControllerActionInvoker
        {
            private readonly Type controllerActionType;

            public HttpMethodActionInvoker(Type controllerActionType)
            {
                this.controllerActionType = controllerActionType;
            }

            protected override ControllerDescriptor GetControllerDescriptor(ControllerContext controllerContext)
            {
                return new HttpMethodControllerDescriptor(controllerActionType);
            }

            private class HttpMethodControllerDescriptor : ReflectedControllerDescriptor
            {
                public HttpMethodControllerDescriptor(Type type)
                    : base(type)
                {
                }

                public override ActionDescriptor FindAction(ControllerContext controllerContext, string actionName)
                {
                    var overrideHeader = controllerContext.HttpContext.Request.Headers["X-Alternate-Verb"];
                    var methodName = overrideHeader ?? controllerContext.HttpContext.Request.HttpMethod;
                    var method = ControllerType.GetMethod(Capitalize(methodName));
                    return method != null ? new ReflectedActionDescriptor(method, actionName, this) : null;
                }

                private static string Capitalize(string str)
                {
                    return str.ToUpper()[0] + str.ToLower().Substring(1);
                }
            }
        }
    }
    public class MvcActionInfo
    {
        public string Area { get; private set; }
        public string Controller { get; private set; }
        public string Action { get; private set; }

        public MvcActionInfo(Type actionType)
        {
            if (string.IsNullOrEmpty(actionType.FullName)) { throw new ArgumentException("Cannot use anonymous classes as mvc actions"); }

            var parts = actionType.FullName.Split(new[] { '.' });
            Action = parts[parts.Length - 1];
            Controller = parts[parts.Length - 2];
            if (parts.Length > 2 && parts[parts.Length - 3] != "Controllers")
            {
                Area = parts[parts.Length - 3];
            }
        }
    }
}