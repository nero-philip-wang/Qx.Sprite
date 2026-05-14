// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------
namespace Atos.China.Siemens.Offer
{
    using Demo.Services.Offer;
    using Qx.Sprite.Core;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    [ServiceType(typeof(IProcOfferServiceBase), "00010002")]
    [ServiceType(typeof(IProcOfferServiceEA))]
    public class ProcOfferServiceEA :  ProcOfferServiceBase, IProcOfferServiceEA, IScoped
    {
        public ProcOfferServiceEA()
        {
            ProcessNextStepService = new ProcessNextStepEA();
        }

        public override Task<object> Get(Guid id)
        {
            return Task.FromResult((object)new { Name = "this is ea" });
        }
    }
}
