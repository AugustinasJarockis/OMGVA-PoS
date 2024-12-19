import React, { useState, useEffect } from 'react';
import { BrowserRouter as Router, Route, Routes, Navigate } from 'react-router-dom';
import LoginPage from './pages/LoginPage';
import BusinessPage from './pages/Business pages/BusinessPage';
import SelectBusinessPage from './pages/Business pages/SelectBusinessPage';
import UpdateBusinessPage from './pages/Business pages/UpdateBusinessPage';
import './App.css';
import { getTokenBusinessId, getTokenRole, isTokenValid } from './utils/tokenUtils';
import CreateBusinessPage from './pages/Business pages/CreateBusinessPage';
import TaxListPage from './pages/Tax pages/TaxListPage';
import CreateTaxPage from './pages/Tax pages/CreateTaxPage';
import UpdateTaxPage from './pages/Tax pages/UpdateTaxPage';
import UpdateUserPage from './pages/UserPages/UpdateUserPage';
import UserDetailsPage from './pages/UserPages/UserDetailsPage';
import CreateUserPage from './pages/UserPages/CreateUserPage';
import UserListPage from './pages/UserPages/UserListPage';
import GiftcardListPage from './pages/GiftcardPage/GiftcardListPage';
import CreateGiftcardPage from './pages/GiftcardPage/CreateGiftcardPage';
import ItemListPage from './pages/Item pages/ItemListPage';
import ItemCategoryPage from './pages/Item pages/ItemCategoryPage';
import UpdateItemPage from './pages/Item pages/UpdateItemPage';
import SelectItemTaxPage from './pages/Item pages/SelectItemTaxPage';
import CreateItemVariationPage from './pages/Item variation pages/CreateItemVariationPage';
import UpdateItemVariationPage from './pages/Item variation pages/UpdateItemVariationPage';
import CreateItemPage from './pages/Item pages/CreateItemPage';
import DiscountListPage from './pages/Discount pages/DiscountListPage';
import CreateDiscountPage from './pages/Discount pages/CreateDiscountPage';
import UpdateDiscountPage from './pages/Discount pages/UpdateDiscountPage';
import HomePage from './pages/Homepage';
import ReservationsListPage from './pages/Reservation pages/ReservationsListPage';
import ReservationUpdatePage from './pages/Reservation pages/ReservationUpdatePage';
import ReservationDetailsPage from './pages/Reservation pages/ReservationDetailsPage';
import EmployeeSchedulesPage from './pages/Schedule pages/EmployeeSchedulesPage';
import CreateSchedulePage from './pages/Schedule pages/CreateSchedulePage';
import UpdateSchedulePage from './pages/Schedule pages/UpdateSchedulePage';
import ReservationCreatePage from './pages/Reservation pages/ReservationCreatePage';
import OrderListPage from './pages/Order pages/OrderListPage';
import OrderPage from './pages/Order pages/OrderPage';
import OrderItemSelectCategoryListPage from './pages/Order pages/OrderItemSelectionCategoryPage';
import OrderItemSelectionListPage from './pages/Order pages/OrderItemSelectionPage';
import SelectOrderItemVariationsPage from './pages/Order pages/SelectOrderItemVariationsPage';
import CreateOrderDiscountPage from './pages/Discount pages/CreateOrderDiscountPage';

const App: React.FC = () => {
    const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);

    const checkTokenValidity = () => {
        const token = localStorage.getItem('authToken');
        if (token && isTokenValid(token)) {
            setIsAuthenticated(true);
        } else {
            localStorage.removeItem('authToken');
            setIsAuthenticated(false);
        }
    };

    useEffect(() => {
        checkTokenValidity();
    }, []);

    const handleLoginSuccess = () => {
        checkTokenValidity();
    };

    const handleLogout = () => {
        localStorage.removeItem('authToken');
        setIsAuthenticated(false);
    };

    const getRole = () => {
        const token = localStorage.getItem('authToken');
        if (token) {
            return getTokenRole(token);
        }
        return null;
    };

    return (
        <Router>
            <div className="app-container">
                <Routes>
                    {!isAuthenticated ? (
                        <Route path="/" element={<LoginPage onLoginSuccess={handleLoginSuccess} />} />
                    ) : (
                        <>
                            <Route path="/business" element={<SelectBusinessPage token={localStorage.getItem('authToken')} onLogout={handleLogout} />} />
                            <Route path="/business/:id" element={<BusinessPage token={localStorage.getItem('authToken')} onLogout={handleLogout} />} />
                            <Route path="/business/create" element={<CreateBusinessPage token={localStorage.getItem('authToken')} />} />
                            <Route path="/business/update/:id" element={<UpdateBusinessPage token={localStorage.getItem('authToken')} />} />
                            <Route path="/discount" element={<DiscountListPage />} />
                            <Route path="/discount/create" element={<CreateDiscountPage />} />
                            <Route path="/discount/update/:id" element={<UpdateDiscountPage />} />
                            <Route path="/item" element={<ItemListPage token={localStorage.getItem('authToken')} />} />
                            <Route path="/item/create" element={<CreateItemPage />} />
                            <Route path="/item/group" element={<ItemCategoryPage/>} />
                            <Route path="/item/:id" element={<UpdateItemPage/>} />
                            <Route path="/item/:id/taxes" element={<SelectItemTaxPage/>} />
                            <Route path="/item/:id/item-variation/create" element={<CreateItemVariationPage/>} />
                            <Route path="/item/:itemId/item-variation/update/:id" element={<UpdateItemVariationPage/>} />
                            <Route path="/order" element={<OrderListPage />} />
                            <Route path="/order/:id" element={<OrderPage />} />
                            <Route path="/order/:id/discount" element={<CreateOrderDiscountPage />} />
                            <Route path="/order/:id/add-items" element={<OrderItemSelectionListPage />} />
                            <Route path="/order/:id/add-items/group" element={<OrderItemSelectCategoryListPage />} />
                            <Route path="/order/:id/add-items/:itemId/variations" element={<SelectOrderItemVariationsPage />} />
                            <Route path="/tax" element={<TaxListPage token={localStorage.getItem('authToken')} />} />
                            <Route path="/tax/create" element={<CreateTaxPage token={localStorage.getItem('authToken')} />} />
                            <Route path="/tax/update/:id" element={<UpdateTaxPage token={localStorage.getItem('authToken')} />} />
                            <Route path="/user/:id" element={<UserDetailsPage />} />
                            <Route path="/user/update/:id" element={<UpdateUserPage />} />
                            <Route path="/user/business" element={<UserListPage />} />
                            <Route path="/user/create" element={<CreateUserPage />} />
                            <Route path="/giftcard" element={<GiftcardListPage />} />
                            <Route path="/giftcard/create" element={<CreateGiftcardPage />} />
                            <Route path="/reservation/employee/:id" element={<ReservationsListPage />} />
                            <Route path="/reservation/:id" element={<ReservationDetailsPage />} />
                            <Route path="/reservation/update/:id" element={<ReservationUpdatePage />} />
                            <Route path="/order/:orderId/reservation/create/item/:itemId/employee/:employeeId" element={<ReservationCreatePage />} />
                            <Route path="/schedules/:id" element={<EmployeeSchedulesPage />} />
                            <Route path="/schedules/create/:id" element={<CreateSchedulePage />} />
                            <Route path="/schedules/update/:id" element={<UpdateSchedulePage />} />
                            <Route path="/home" element={<HomePage onLogout={handleLogout} />} />
                            {localStorage.getItem('authToken') !== null && (getRole() === "Admin")
                                ? (<Route path="*" element={<Navigate to="/business" />} />)
                                :
                                (getRole() === "Owner" ? <Route path="*" element={<Navigate to={'/business/'+getTokenBusinessId(localStorage.getItem('authToken') ?? "")} />} /> :
                                    <Route path="*" element={<Navigate to="/home" />} />
                                )
                             }
                        </>
                    )}
                    {!isAuthenticated && <Route path="*" element={<Navigate to="/" />} />}
                </Routes>
            </div>
        </Router>
    );
};

export default App;
