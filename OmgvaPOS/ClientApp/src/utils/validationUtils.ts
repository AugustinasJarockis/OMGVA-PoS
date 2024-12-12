const validateName = (name: string): boolean => {
    const regex = /^[a-zA-Z][a-zA-Z ]*$/;
    return regex.test(name) && name.length <= 150;
};

const validateUsername = (username: string): boolean => {
    const regex = /^[a-zA-Z0-9][a-zA-Z0-9\S]*$/;
    return regex.test(username) && username.length >= 8 && username.length <= 30;
};

const validatePassword = (password: string): boolean => {
    const regex = /^[a-zA-Z0-9](?=.*[a-zA-Z])(?=.*[0-9])[a-zA-Z0-9\W]*$/;
    return regex.test(password) && password.length >= 8 && password.length <= 100;
};

export { validateName, validateUsername, validatePassword }