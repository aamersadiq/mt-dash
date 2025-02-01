import { configureStore } from '@reduxjs/toolkit';
import authReducer from './store/auth/authSlice.js';
import accountReducer from './store/account/accountSlice';

const store = configureStore({
  reducer: {
    auth: authReducer,
    account: accountReducer,
  },
});

export default store;
