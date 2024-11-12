﻿namespace OMGVA_PoS.Data_layer.Models
{
    public class TaxItem
    {
        public long Id { get; set; }
        public long TaxId { get; set; }
        public long ItemId { get; set; }

        // navigational properties
        // for foreign keys
        public Tax Tax { get; set; }
        public Item Item {  get; set; }
    }
}
