using System.Web.Mvc;

namespace SSG.Web.Framework.Mvc
{
    public partial class BaseSSGModel
    {
        public virtual void BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
        }
    }
    public partial class BaseSSGEntityModel : BaseSSGModel
    {
        public virtual int Id { get; set; }
    }
}
