// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------
namespace Atos.China.Siemens.Offer
{
    using Qx.Sprite.Core;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    [ServiceType(typeof(IProcOfferServiceBase), "0001")]
    public class ProcOfferServiceBase : IProcOfferServiceBase, IScoped
    {
        protected ProcessNextStep ProcessNextStepService = new ProcessNextStep();


        public virtual Task<object> Get(Guid id)
        {
            return Task.FromResult((object)new { Name = "this is base" });
        }

        public virtual Task<Guid> Save(object offer)
        {
            return Task.FromResult(Guid.NewGuid());
        }

        public virtual Task<Guid> Submit(object offer)
        {
            return Task.FromResult(Guid.NewGuid());
        }

        public virtual Task Delete(Guid id)
        {
            return Task.CompletedTask;
        }
    }
}
