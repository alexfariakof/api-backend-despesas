﻿
namespace despesas_backend_api_net_core.Domain.Entities
{
    public class Categoria : BaseModel
    {        
        public string Descricao { get; set; }
        public int? IdUsuario { get; set; }
        public virtual Usuario Usuario { get; set; }
        public int IdTipoCategoria { get; set; }
        public virtual TipoCategoria TipoCategoria { get; set; }


    }
}
