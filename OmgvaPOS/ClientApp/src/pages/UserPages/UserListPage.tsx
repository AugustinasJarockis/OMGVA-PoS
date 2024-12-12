import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../contexts/AuthContext';
import { deleteUser, getBusinessUsers } from '../../services/userService';
import { getTokenBusinessId, getTokenRole, getTokenUserId } from '../../utils/tokenUtils';
import DeletableUpdatableListItem from '../../components/List/DeletableUpdatableListItem';
import ClickableListItem from '../../components/List/ClickableListItem';

const UserListPage: React.FC = () => {
    const [listItems, setListItems] = useState<Array<JSX.Element>>();
    const [error, setError] = useState<string | null>(null);
    const navigate = useNavigate();
    const { authToken } = useAuth();

    const onDelete = async (id: string) => {
        try {
            const error = await deleteUser(authToken, id);

            if (error) {
                setError("An error occurred while deleting user: " + error);
                return;
            }

            if (!listItems) {
                getUsers();
            }

            if (listItems) {
                const newList = listItems.filter((item) => item.key != id);
                setListItems(newList);
            }
        }
        catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    }

    const getUsers = async () => {
        setError(null);

        try {
            const { result, error } = await getBusinessUsers(authToken, getTokenBusinessId(authToken ?? ''));

            if (!result) {
                setError('Problem acquiring taxes: ' + error);
            }
            else {
                setListItems(result.map(user =>
                    <DeletableUpdatableListItem
                        key={user.Id}
                        id={user.Id ?? ''}
                        text={user.Name ?? '-'}
                        updateUrl={"/user/update/" + user.Id}
                        deleteFunction={onDelete}
                        disableDelete={user.Id === getTokenUserId(authToken ?? '') || user.HasLeft}
                        object={user}
                    />));
            }
        } catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    }

    const goToBusiness = async () => {
        if (authToken) {
            const businessId = getTokenBusinessId(authToken);
            navigate('/business/' + businessId);
        }
    };

    useEffect(() => {
        if (authToken) {
            const role = getTokenRole(authToken);
            if (role === "Employee") {
                navigate('/');
                return;
            }
            getUsers();
        }
        else {
            setError("You have to authenticate first!");
        }
    }, [authToken]);

    return (
        <div>
            <header>
                <button onClick={goToBusiness}>Return to business</button>
            </header>
            <h1>Business users</h1>
            <div className="tax-list-container">
                {listItems}
                <div className="create-button-wrapper" >
                    <ClickableListItem key="create" text="Create a new user" url={'/user/create'} />
                </div>
            </div>
            {error && <p className="error-message">{error}</p>}
        </div>
    );
};

export default UserListPage;
