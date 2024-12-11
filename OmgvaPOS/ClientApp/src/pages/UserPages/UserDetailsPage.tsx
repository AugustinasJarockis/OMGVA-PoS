import { useEffect, useState } from "react";
import { useLocation, useNavigate, useParams } from "react-router-dom";
import { UserResponse, getUser } from "../../services/userService";
import { getTokenBusinessId, getTokenRole, getTokenUserId } from "../../utils/tokenUtils";

interface UserPageProps {
    token: string | null
}

const UserPage: React.FC<UserPageProps> = ({ token: authToken }) => {
    const [error, setError] = useState<string | null>(null);
    const [user, setUser] = useState<UserResponse>();
    const { state } = useLocation();
    const { id } = useParams();
    const navigate = useNavigate();

    const loadUser = async () => {
        setError(null);

        try {
            if (!id) {
                setError('Could not identify the user');
                return;
            }
            const { result, error } = await getUser(authToken, id);

            if (!result) {
                setError('Problem acquiring user: ' + error);
            }
            else {
                setUser(result);
            }
        } catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        }
    }

    const roleMap: { [key: string]: string } = {
        2: 'Admin',
        1: 'Owner',
        0: 'Employee',
    };

    const handleUpdateUserOnclick = async () => {
        navigate("/user/update/" + id, { state: { user: user } });
    }

    const goToBusiness = async () => {
        if (authToken) {
            const businessId = getTokenBusinessId(authToken)
            navigate('/business/' + businessId);
        }
    }

    useEffect(() => {
        if (state && state.user) {
            setUser(state.user);
        }
        else {
            loadUser();
        }

        if (authToken) {
            const role = getTokenRole(authToken);
            const userId = getTokenUserId(authToken);
            if (!(userId === id || role === "Admin" || role === "Owner")) {
                navigate('/');
            }
        }
        else {
            setError("You have to authenticate first!");
        }
    }, []);

    return (
        <div>
            {user ? (
                <>
                    <button onClick={goToBusiness}>Return to business</button>
                    <h1>{user.Name}</h1>
                    <section>
                        <p>Username: {user.Username}</p>
                        <p>Email: {user.Email}</p>
                        <p>Role: {user && user.Role && roleMap[user.Role] ? roleMap[user.Role] : 'Unknown Role'}</p>
                    </section>
                    <button onClick={handleUpdateUserOnclick}>Update information</button>
                </>
            )
                : <p className="error-message">{error}</p>
            }
        </div>
    );
};

export default UserPage;

