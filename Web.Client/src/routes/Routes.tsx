import Home from "@/components/Home";
import { Route, Routes } from "react-router-dom";
import { UnauthenticatedRoute } from "./UnauthenticatedRoute";
import Login from "@/components/Login";
import Register from "@/components/Register";
import ForgotPassword from "@/components/ForgotPassword";
import ResetPassword from "@/components/ResetPassword";
import { ProtectedRoute } from "./ProtectedRoute";
import { AuthenticatedRoute } from "./AuthenticatedRoute";
import { AccountLayout } from "@/components/layouts";
import { OrdersHistoryPage, AddNewPaymentCard, ChangePasswordPage, EditProfilePage, PaymentCardsPage, ProfilePage } from "@/pages/account";
import { Claims } from "../resources";
import { NotFoundPage } from "@/pages";
import Shops from "@/components/shop/Shops";
import AddShop from "@/components/shop/AddShop";
import Shop from "@/components/shop/Shop";
import AddBouquet from "@/components/bouquet/Add-Bouquet";
import { ConditionRoute } from "./ConditionRoute";
import Cart from "@/components/cart/Cart";
import { useCart } from "@/stateProviders/cartContext";
import Order from "@/components/cart/Order";


export default function Router() {
    const { bouquetsCount } = useCart();

    return (
        <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/shop" element={<Shop />} />
            <Route path="/shops" element={<Shops />} />
            <Route
                path="/add-bouquet"
                element={
                    <ProtectedRoute requiredClaims={[Claims.PERMISSION_BOUQUET_ADD]}>
                        <AddBouquet />
                    </ProtectedRoute>
                }
            />
            <Route
                path="/add-flower-shop"
                element={
                    <ProtectedRoute requiredClaims={[Claims.PERMISSION_SHOP_ADD]}>
                        <AddShop />
                    </ProtectedRoute>
                }
            />
            <Route
                path="/cart"
                element={
                    <ConditionRoute condition={bouquetsCount > 0}>
                        <Cart />
                    </ConditionRoute>
                }
            />
            <Route element={<UnauthenticatedRoute />}>
                <Route path="/login" element={<Login />} />
                <Route path="/register" element={<Register />} />
                <Route path="/forgot-password" element={<ForgotPassword />} />
                <Route path="/reset-password" element={<ResetPassword />} />
            </Route>

            <Route element={<AuthenticatedRoute />} >
                <Route
                    path="/order"
                    element={<Order />}
                />
                <Route
                    path="/profile"
                    element={
                        <AccountLayout>
                            <ProfilePage />
                        </AccountLayout>
                    }
                />
                <Route
                    path="/edit-profile"
                    element={
                        <AccountLayout>
                            <EditProfilePage />
                        </AccountLayout>
                    }
                />
                <Route
                    path="/payment-cards"
                    element={
                        <AccountLayout>
                            <PaymentCardsPage />
                        </AccountLayout>
                    }
                />
                <Route
                    path="/payment-cards/add"
                    element={
                        <AccountLayout>
                            <AddNewPaymentCard />
                        </AccountLayout>
                    } />
                <Route
                    path="/change-password"
                    element={
                        <AccountLayout>
                            <ChangePasswordPage />
                        </AccountLayout>
                    }
                />
                <Route
                    path="/orders-history"
                    element={
                        <AccountLayout>
                            <OrdersHistoryPage />
                        </AccountLayout>
                    }
                />
            </Route>
            <Route path="*" element={<NotFoundPage />} />
        </Routes>
    )
}