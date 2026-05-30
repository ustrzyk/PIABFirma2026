using System;
using System.Collections.Generic;
using System.Text;

using Firma.Data.Data;

namespace Firma.Services.Abstrakcja
{
    public abstract class BaseService
    {
        protected readonly FirmaContext _context;

        protected BaseService(FirmaContext context)
        {
            _context = context;
        }
    }
}