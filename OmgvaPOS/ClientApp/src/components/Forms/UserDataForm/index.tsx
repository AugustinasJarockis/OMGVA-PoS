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
            Username: { value: string }
            Name: { value: string }
            Email: { value:string }
            Password: { value: string }
            Role: { value: string }
        }

        const user: UpdateUser = {
            Username: formElements.Username.value === '' ? undefined : formElements.Username.value,
            Name: formElements.Name.value === '' ? undefined : formElements.Name.value,
            Email: formElements.Email.value === '' ? undefined : formElements.Email.value,
            Password: formElements.Password.value === '' ? undefined : formElements.Password.value,
            Role: formElements.Role.value === '' ? undefined : formElements.Role.value,

        };

        props.onSubmit(user);
    }

    return (
        <div>
            <h1>Update User</h1>
            <form onSubmit={handleSubmission}>
                <label htmlFor="name">Name</label>
                <input type="text" id="name" name="name" placeholder={props.user?.Name} /><br /><br />
                <label htmlFor="username">Username</label>
                <input type="text" id="username" name="username" placeholder={props.user?.Username} /><br /><br />
                <label htmlFor="email">Email</label>
                <input type="email" id="email" name="email" placeholder={props.user?.Email} /><br /><br />
                <label htmlFor="password">Password</label>
                <input type="text" id="password" name="password" /><br /><br />
                <select name="role" id="role" hidden={props.roleFromToken === "Employee"}>
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
