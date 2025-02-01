import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { fetchAccounts } from '../../store/account/accountSlice';
import { useNavigate } from 'react-router-dom';
import { logout } from '../..//store/auth/authSlice';

function AccountDashboard() {
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const userId = useSelector((state) => state.auth.userId);
  const roles = useSelector((state) => state.auth.roles);
  const accounts = useSelector((state) => state.account.accounts);
  const [selectedAccount, setSelectedAccount] = useState(null);

  const hasAccountManageRole = roles.includes('AccountManage');

  useEffect(() => {
    if (userId) {
      dispatch(fetchAccounts(userId));
    }
  }, [dispatch, userId]);

  const handleViewTransactions = (account) => {
    setSelectedAccount(account);
  };

  const formatDate = (dateString) => {
    const date = new Date(dateString);
    return date.toISOString().split('T')[0];
  };

  const formatBalance = (balance) => {
    return new Intl.NumberFormat('en-AU', {
      style: 'currency',
      currency: 'AUD',
      minimumFractionDigits: 2,
      maximumFractionDigits: 2,
    }).format(balance);
  };

  return (
    <div className="container mt-4">
      <h2>Account Dashboard</h2>
      {hasAccountManageRole && (
            <button
              className="btn btn-primary mt-3"
              onClick={() => navigate('/transfer')}
            >
              Transfer Between Accounts
            </button>
          )}
      <div className="list-group mt-4">
        {accounts.map((account) => (
          <div key={account.id} className="list-group-item">
            <div className="d-flex justify-content-between align-items-center">
              <div>
                <strong>Account #{account.id}</strong> - Balance: {formatBalance(account.balance)}
              </div>
              <button
                className="btn btn-secondary"
                onClick={() => handleViewTransactions(account)}
              >
                View Transactions
              </button>
            </div>
          </div>
        ))}
      </div>
      {selectedAccount && (
        <div className="mt-4">
          <h3>Transactions for Account #{selectedAccount.id}</h3>
          <ul className="list-group">
            {selectedAccount.transactions.map((transaction) => (
              <li key={transaction.id} className="list-group-item">
                {transaction.type} - {formatBalance(transaction.amount)} on {formatDate(transaction.transactionDate)}
              </li>
            ))}
          </ul>
        </div>
      )}
    </div>
  );
}

export default AccountDashboard;
