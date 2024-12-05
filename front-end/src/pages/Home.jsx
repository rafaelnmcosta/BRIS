import React from 'react';
import { useAuth } from '../services/AuthContext';
import TemplateHome from '../components/templates/TemplateHome';

const Home = () => {
  const { userType } = useAuth();
  return <TemplateHome userType={userType} />;
};

export default Home;
