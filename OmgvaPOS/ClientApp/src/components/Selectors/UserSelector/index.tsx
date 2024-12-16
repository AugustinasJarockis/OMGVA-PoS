import React, { useEffect, useState } from 'react';
import './UserSelector.css';
import { useAuth } from '../../../contexts/AuthContext';
import { getBusinessUsers } from '../../../services/userService';
import { getTokenBusinessId } from '../../../utils/tokenUtils';

interface UserSelectorProps {
    required?: boolean;
    current?: string;
}

const UserSelector: React.FC<UserSelectorProps> = (props: UserSelectorProps) => {
    const [error, setError] = useState<string | null>(null);
    const [listItems, setListItems] = useState<Array<JSX.Element>>();
    const { authToken } = useAuth();

    const getUsers = async () => {
        setError(null);

        try {
            const { result, error } = await getBusinessUsers(authToken, getTokenBusinessId(authToken ?? ''));

            if (!result) {
                setError('Problem acquiring user: ' + error);
            }
            else {
                setListItems(result.map(user =>
                    <option key={user.Id} value={user.Id}>
                        { user.Name }
                    </option>));
            }
        } catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    }

    useEffect(() => {
        if (!authToken) {
            setError("You have to authenticate first!");
        }
        else {
            getUsers();
        }
    }, []);

    return (
        <div className="selector-container">
            <select className="wider-select" name="userId" id="userId" required={props?.required} defaultValue={props.current ? props.current : '' }>
                <option hidden disabled value=''> -- select an option -- </option>
                {listItems}
            </select>
        </div>
        || error
    );
};

export default UserSelector;