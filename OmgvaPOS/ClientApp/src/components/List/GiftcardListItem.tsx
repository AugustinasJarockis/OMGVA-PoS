import React from 'react';
import './GiftcardListItem.css';

interface GiftcardListItemProps {
    text: string;
    value: number;
    balance: number;
}

const GiftcardListItem: React.FC<GiftcardListItemProps> = (props: GiftcardListItemProps) => {
    return (
        <div className="giftcard-list-item">
            <table>
                <tbody>
                    <tr>
                        <td>Code: {props.text}</td>
                        <td>Value: {props.value.toFixed(2)}</td>
                        <td>Balance: {props.balance.toFixed(2)}</td>
                    </tr>
                </tbody>
            </table>
        </div>
    );
};

export default GiftcardListItem;