import axios from 'axios';
import { SimpleDiscount } from './discountService';

export interface OrderItem {
    Id: string,
    TotalPrice: number,
    UnitPriceNoDiscount: number,
    TaxPercent: string,
    ItemId: string,
    ItemName: string,
    Quantity: number,
    Discount?: SimpleDiscount,
    Variations?: Array<OrderItemVariation>
}

export interface OrderItemVariation {
    Id: string,
    ItemVariationId: string,
    ItemVariationName: string,
    ItemVariationGroup: string,
    PriceChange: number
}

export interface CreateOrderItemRequest {
    Quantity: number,
    ItemId: string,
    ItemVariationIds: Array<string>
}

export interface UpdateOrderItemRequest {
    Quantity: number
}