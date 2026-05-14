// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------
namespace Atos.China.Siemens.Offer
{
    using System;
    using System.Threading.Tasks;

    public interface IProcOfferServiceBase
    {
        Task Delete(Guid id);
        Task<object> Get(Guid id);
        Task<Guid> Save(object offer);
        Task<Guid> Submit(object offer);
    }
}