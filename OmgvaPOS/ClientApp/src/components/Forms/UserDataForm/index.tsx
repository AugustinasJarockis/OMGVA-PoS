import React from 'react';
import { UpdateUser, UserResponse } from '../../../services/userService';

interface UserDataFormProps {
    user?: UserResponse;
    roleFromToken?: string;
    idFromToken?: string;
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
            email: { value: string }
            password: { value: string }
            role: { value: string }
        };

        const user: UpdateUser = {
            Username: formElements.username.value || undefined,
            Name: formElements.name.value || undefined,
            Email: formElements.email.value || undefined,
            Password: formElements.password.value || undefined,
            Role: formElements.role.value || undefined,
        };
        props.onSubmit(user);
    }

    const shouldHide = props.roleFromToken !== "Employee" && (props.roleFromToken !== "Admin" && props.idFromToken !== props.user?.Id);

    return (
        <div>
            <form onSubmit={handleSubmission}>
                <label htmlFor="name">Name</label>
                <input type="text" id="name" name="name" placeholder={props.user?.Name} defaultValue='' /><br /><br />
                <label htmlFor="username">Username</label>
                <input type="text" id="username" name="username" placeholder={props.user?.Username} defaultValue='' /><br /><br />
                <label htmlFor="email">Email</label>
                <input type="email" id="email" name="email" placeholder={props.user?.Email} defaultValue={props.user?.Email} required/><br /><br />
                <label htmlFor="password">Password</label>
                <input type="password" id="password" name="password" hidden={props.idFromToken === props.user?.Id} defaultValue=''/><br /><br />
                <select name="role" id="role" style={{ display: shouldHide ? 'block' : 'none' }}>
                    <option value="">Select a role</option>
                    <option value="1">Owner</option>
                    <option value="0">Employee</option>
                </select>
                <input type="submit" value={props.submitText ? props.submitText : "Submit"} />
            </form>
        </div>
    );
};

export default UserDataForm;
