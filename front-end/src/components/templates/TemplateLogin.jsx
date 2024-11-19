import React from 'react';
import FormLogin from '../organisms/FormLogin';

const TemplateLogin = ({ handleLogin }) => {
  return (
    <div className="h-screen flex items-center justify-center">
      <FormLogin handleLogin={handleLogin} />
    </div>
  );
};

export default TemplateLogin;
