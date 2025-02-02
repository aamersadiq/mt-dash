import React from 'react';
import { render, screen } from '@testing-library/react';
import { Provider } from 'react-redux';
import configureStore from 'redux-mock-store';
import Header from '../common/Header';

// Create a mock store
const mockStore = configureStore([]);
const store = mockStore({
  auth: {
    isLoggedIn: false,
    userId: null,
    token: null,
    roles: [],
  },
});

describe('Header', () => {
  test('renders Header component correctly and has text "My Bank"', () => {
    render(
      <Provider store={store}>
        <Header />
      </Provider>
    );
    expect(screen.getByText('My Bank')).toBeInTheDocument();
  });
});
