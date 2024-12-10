import React, { useState } from 'react';
import axios from 'axios';

const CreateUserPage: React.FC<{ token: string | null }> = ({ token }) => {
    const [formData, setFormData] = useState({
        name: '',
        username: '',
        email: '',
        password: '',
        role: 'Employee',
    });

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            const response = await axios.post('/user', formData, {
                headers: { Authorization: `Bearer ${token}` },
            });
            console.log('User created:', response.data);
        } catch (error) {
            console.error('Error creating user:', error);
        }
    };

    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
    };

    return (
        <div>
            <h1>Create User</h1>
            <form onSubmit={handleSubmit}>
                <input name="name" placeholder="Name" onChange={handleChange} />
                <input name="username" placeholder="Username" onChange={handleChange} />
                <input name="email" type="email" placeholder="Email" onChange={handleChange} />
                <input name="password" type="password" placeholder="Password" onChange={handleChange} />
                <select name="role" onChange={handleChange}>
                    <option value="Admin">Admin</option>
                    <option value="Owner">Owner</option>
                    <option value="Employee">Employee</option>
                </select>
                <button type="submit">Create</button>
            </form>
        </div>
    );
};

export default CreateUserPage;
