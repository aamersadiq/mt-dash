import React from 'react';
import { render, screen } from '@testing-library/react';
import Footer from '../common/Footer';

describe('Footer', () => {
  test('should render', () => {
    render(<Footer />);
    expect(screen.getByText('© 2025 My Bank')).toBeInTheDocument();
  });
});
