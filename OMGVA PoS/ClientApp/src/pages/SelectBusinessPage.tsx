import React, { useState, useEffect } from 'react';
import { getAllBusinesses } from '../services/businessService';

interface SelectBusinessPageProps {
}

const SelectBusinessPage: React.FC<SelectBusinessPageProps> = ({}) => {
    const [listItems, setListItems] = useState<Array<JSX.Element>>();
    const [error, setError] = useState<string | null>(null);
    //const [isLoading, setIsLoading] = useState<boolean>(false);

    useEffect(() => {
        getBusinesses();
    }, []);

    const getBusinesses = async () => {
        setError(null);
        //setIsLoading(true);

        try {
            const { result, error } = await getAllBusinesses();

            if (!result) {
                setError('Problem acquiring businesses: ' + error);
            }
            else {
                setListItems(result.map(business => <li key={ business.id }>{ business.name }</li>));
            }
        } catch (err: any) {
            setError(err.message || 'An unexpected error occurred.');
        } finally {
            //setIsLoading(false);
        }
    }
    
    //const handleLogin = async (e: React.FormEvent) => {
    //    e.preventDefault();
    //    setError(null);
    //    setIsLoading(true);

    //    const loginRequest = { username, password };

    //    try {
    //        const response = await login(loginRequest);
    //        if (response.isSuccess && response.token !== undefined) {
    //            localStorage.setItem('authToken', response.token);
    //            onLoginSuccess();
    //        } else {
    //            setError(response.message);
    //        }
    //    } catch (err: any) {
    //        setError(err.message || 'An unexpected error occurred.');
    //    } finally {
    //        setIsLoading(false);
    //    }
    //};
    //const businesses: List<Business> = 

    return (
        <div className="business-list-container">
            <h1>Select the business to open</h1>
            <ul>{listItems}</ul>
            {error && <p className="error-message">{error}</p>}
            {/*<form onSubmit={handleLogin} className="login-form">*/}
            {/*    <div className="input-group">*/}
            {/*        <label htmlFor="username">Username</label>*/}
            {/*        <input*/}
            {/*            type="text"*/}
            {/*            id="username"*/}
            {/*            value={username}*/}
            {/*            onChange={(e) => setUsername(e.target.value)}*/}
            {/*            required*/}
            {/*        />*/}
            {/*    </div>*/}
            {/*    <div className="input-group">*/}
            {/*        <label htmlFor="password">Password</label>*/}
            {/*        <input*/}
            {/*            type="password"*/}
            {/*            id="password"*/}
            {/*            value={password}*/}
            {/*            onChange={(e) => setPassword(e.target.value)}*/}
            {/*            required*/}
            {/*        />*/}
            {/*    </div>*/}
            {/*    {error && <p className="error-message">{error}</p>}*/}
            {/*    <button type="submit" disabled={isLoading}>*/}
            {/*        {isLoading ? 'Logging in...' : 'Login'}*/}
            {/*    </button>*/}
            {/*</form>*/}
        </div>
    );
};

export default SelectBusinessPage;

