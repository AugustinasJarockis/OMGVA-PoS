import { useEffect, useState } from "react";
import { useLocation, useNavigate, useParams } from "react-router-dom";
import { UserResponse, getUser } from "../../services/userService";
import { getTokenRole, getTokenUserId } from "../../utils/tokenUtils";

interface UserPageProps {
    token: string | null
}

const UserPage: React.FC<UserPageProps> = ({ token: authToken }) => {
    const [error, setError] = useState<string | null>(null);
    const [role, setRole] = useState<string>('');
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

    const handleUpdateUserOnclick = async () => {
        navigate("/user/update/" + id, { state: { user: user } });
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
            setRole(role);
        }
        else {
            setError("You have to authenticate first!");
        }
    }, []);

    return (
        <div>
            {user ? (
                <>
                    <h1>{user.name}</h1>
                    <section>
                        <p>Username: {user.username}</p>
                        <p>Email: {user.email}</p>
                        <p>Role: {user.role}</p>
                        <p>Account active: {!user.hasLeft}</p>
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

