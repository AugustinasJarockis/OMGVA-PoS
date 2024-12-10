import React from 'react';
import { UpdateUser, UserResponse } from '../../../services/userService';

interface UserDataFormProps {
    user?: UserResponse;
    roleFromToken?: string;
    submitText?: string;
    onSubmit: (user: UpdateUser) => void;
    required?: boolean;
}

const UserDataForm: React.FC<UserDataFormProps> = (props: UserDataFormProps) => {
    const handleSubmission = async (e: React.SyntheticEvent<HTMLFormElement>) => {
        e.preventDefault();

        const form = e.currentTarget;
        const formElements = form.elements as typeof form.elements & {
            username: { value: string }
            name: { value: string }
            email: { value:string }
            password: { value: string }
            role: { value: string }
        }

        const user: UpdateUser = {
            username: formElements.username.value === '' ? undefined : formElements.username.value,
            name: formElements.name.value === '' ? undefined : formElements.name.value,
            email: formElements.username.value === '' ? undefined : formElements.email.value,
            password: formElements.username.value === '' ? undefined : formElements.password.value,
            role: formElements.username.value === '' ? undefined : formElements.role.value,

        };

        props.onSubmit(user);
    }

    return (
        <div>
            <h1>Update User</h1>
            <form onSubmit={handleSubmission}>
                <label htmlFor="name">Name</label>
                <input type="text" id="name" name="name" placeholder={props.user?.name} /><br /><br />
                <label htmlFor="username">Username</label>
                <input type="text" id="username" name="username" placeholder={props.user?.username} /><br /><br />
                <label htmlFor="email">Email</label>
                <input type="email" id="email" name="email" placeholder={props.user?.email} /><br /><br />
                <label htmlFor="username">Username</label>
                <input type="text" id="username" name="username" placeholder={props.user?.username} /><br /><br />
                <label htmlFor="password">Password</label>
                <input type="password" id="password" name="password" /><br /><br />
                <select name="role" id="role" type="role" hidden={props.roleFromToken === "Employee"}>
                    <option value="Admin">Admin</option>
                    <option value="Owner">Owner</option>
                    <option value="Employee">Employee</option>
                </select>

                <input type="submit" value={props.submitText ? props.submitText : "Submit"} />
            </form>
        </div>
    );
};

export default UserDataForm;
