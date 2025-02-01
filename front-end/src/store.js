import { configureStore } from '@reduxjs/toolkit';
import authReducer from './features/auth/authSlice.js';
import accountReducer from './features/account/accountSlice';

const store = configureStore({
  reducer: {
    auth: authReducer,
    account: accountReducer,
  },
});

export default store;
