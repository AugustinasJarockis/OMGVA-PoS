import React, { useState } from 'react';
import { CreateUser, UpdateUser, UserResponse } from '../../../services/userService';
import { getTokenBusinessId, getTokenRole, getTokenUserId } from '../../../utils/tokenUtils';
import { validateName, validatePassword, validateUsername } from '../../../utils/validationUtils';

interface UserDataFormProps {
    token: string;
    user?: UserResponse;
    submitText?: string;
    onSubmitUpdate?: (user: UpdateUser) => void;
    onSubmitCreate?: (user: CreateUser) => void;
    required?: boolean;
}

const CreateUserFrom: React.FC<UserDataFormProps> = (props: UserDataFormProps) => {
    const [errors, setErrors] = useState<any>({});

    const handleSubmission = async (e: React.SyntheticEvent<HTMLFormElement>) => {
        e.preventDefault();

        const form = e.currentTarget;
        const formElements = form.elements as typeof form.elements & {
            username: { value: string };
            name: { value: string };
            email: { value: string };
            password: { value: string };
            role: { value: string };
        };

        const user: CreateUser = {
            Username: formElements.username.value,
            Name: formElements.name.value,
            Email: formElements.email.value,
            Password: formElements.password.value,
            Role: formElements.role.value,
            BusinessId: formElements.role.value === "2" ? null : getTokenBusinessId(props.token),
        };

        const newErrors: any = {};
        if (!validateName(user.Name)) newErrors.name = 'Name is invalid or too long.';
        if (!validateUsername(user.Username)) newErrors.username = 'Username must be 8-30 characters and contain no spaces.';
        if (!validatePassword(user.Password)) newErrors.password = 'Password must be 8-100 characters long and contain both letters and numbers.';

        if (Object.keys(newErrors).length > 0) {
            setErrors(newErrors);
        } else {
            setErrors({});
            if (props.onSubmitCreate) {
                props.onSubmitCreate(user);
            }
        }
    };

    return (
        <div>
            <form onSubmit={handleSubmission}>
                <label htmlFor="name">Name</label>
                <input type="text" id="name" name="name" required />
                {errors.name && <p className="error-message" style={{ color: 'red' }}>{errors.name}</p>}
                <br /><br />

                <label htmlFor="username">Username</label>
                <input type="text" id="username" name="username" required />
                {errors.username && <p className="error-message" style={{ color: 'red' }}>{errors.username}</p>}
                <br /><br />

                <label htmlFor="email">Email</label>
                <input type="email" id="email" name="email" required />
                <br /><br />

                <label htmlFor="password">Password</label>
                <input type="password" id="password" name="password" required />
                {errors.password && <p className="error-message" style={{ color: 'red' }}>{errors.password}</p>}
                <br /><br />

                <label htmlFor="role">Role</label>
                <select name="role" id="role" required>
                    {getTokenRole(props.token) === "Owner" && (
                        <>
                            <option value="1">Owner</option>
                            <option value="0">Employee</option>
                        </>
                    )}
                    {getTokenRole(props.token) === "Admin" && (
                        <>
                            <option value="2">Admin</option>
                            <option value="1">Owner</option>
                            <option value="0">Employee</option>
                        </>
                    )}
                </select><br /><br />

                <input type="submit" value={props.submitText ? props.submitText : "Submit"} />
            </form>
        </div>
    );
};

const UserDataForm: React.FC<UserDataFormProps> = (props: UserDataFormProps) => {
    const [errors, setErrors] = useState<any>({});

    const handleSubmission = async (e: React.SyntheticEvent<HTMLFormElement>) => {
        e.preventDefault();

        const form = e.currentTarget;
        const formElements = form.elements as typeof form.elements & {
            username: { value: string };
            name: { value: string };
            email: { value: string };
            password: { value: string };
            role: { value: string };
        };

        const user: UpdateUser = {
            Username: formElements.username.value || undefined,
            Name: formElements.name.value || undefined,
            Email: formElements.email.value || undefined,
            Password: formElements.password.value || undefined,
            Role: formElements.role.value || undefined,
        };

        const newErrors: any = {};
        if (user.Name && !validateName(user.Name)) newErrors.name = 'Name is invalid or too long.';
        if (user.Username && !validateUsername(user.Username)) newErrors.username = 'Username must be 8-30 characters and contain no spaces.';
        if (user.Password && !validatePassword(user.Password)) newErrors.password = 'Password must be 8-100 characters long and contain both letters and numbers.';

        if (Object.keys(newErrors).length > 0) {
            setErrors(newErrors);
        } else {
            setErrors({});
            if (props.onSubmitUpdate) {
                props.onSubmitUpdate(user);
            }
        }
    };

    const shouldHide = getTokenRole(props.token) !== "Employee" || (getTokenRole(props.token) !== "Admin" && getTokenUserId(props.token) !== props.user?.Id);

    return (
        <div>
            <form onSubmit={handleSubmission}>
                <label htmlFor="name">Name</label>
                <input type="text" id="name" name="name" placeholder={props.user?.Name} defaultValue='' />
                {errors.name && <p className="error-message" style={{ color: 'red' }}>{errors.name}</p>}
                <br /><br />

                <label htmlFor="username">Username</label>
                <input type="text" id="username" name="username" placeholder={props.user?.Username} defaultValue='' />
                {errors.username && <p className="error-message" style={{ color: 'red' }}>{errors.username}</p>}
                <br /><br />

                <label htmlFor="email">Email</label>
                <input type="email" id="email" name="email" placeholder={props.user?.Email} defaultValue={props.user?.Email} required />
                <br /><br />

                <label
                    htmlFor="password"
                    style={{
                        visibility: getTokenUserId(props.token) === props.user?.Id || getTokenRole(props.token) === "Admin" ? 'visible' : 'hidden',
                    }}
                >
                    Password
                </label>
                <input
                    type="password"
                    id="password"
                    name="password"
                    style={{
                        visibility: getTokenUserId(props.token) === props.user?.Id || getTokenRole(props.token) === "Admin" ? 'visible' : 'hidden',
                    }}
                    defaultValue=''
                /><br /><br />
                {errors.password && <p className="error-message" style={{ color: 'red' }}>{errors.password}</p>}
                <br /><br />

                <label htmlFor="role" style={{ display: shouldHide ? 'block' : 'none' }}>Role</label>
                <select name="role" id="role" style={{ display: shouldHide ? 'block' : 'none' }}>
                    <option value="">Select a role</option>
                    <option value="1">Owner</option>
                    <option value="0">Employee</option>
                </select><br /><br />

                <input type="submit" value="Submit" />
            </form>
        </div>
    );
};


export { UserDataForm, CreateUserFrom };