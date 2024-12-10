import React, { useState, useEffect } from 'react';
import axios from 'axios';

const UserListPage: React.FC<{ token: string | null }> = ({ token }) => {
    const [users, setUsers] = useState([]);

    useEffect(() => {
        const fetchUsers = async () => {
            try {
                const response = await axios.get('/user', {
                    headers: { Authorization: `Bearer ${token}` },
                });
                setUsers(response.data);
            } catch (error) {
                console.error('Error fetching users:', error);
            }
        };

        fetchUsers();
    }, [token]);

    return (
        <div>
            <h1>User List</h1>
            <ul>
                {users.map((user: any) => (
                    <li key={user.id}>{user.name} ({user.email})</li>
                ))}
            </ul>
        </div>
    );
};

export default UserListPage;
