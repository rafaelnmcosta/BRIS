import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';

import 'antd/dist/reset.css';

import PrivateRoute from './components/PrivateRoute';
import { AuthProvider } from './services/AuthContext';

import Home from './pages/Home';
import Login from './pages/Login';
import Teste from './pages/Teste';
import Header from './components/molecules/Header';

function App() {
  return (
      <Router>
        <AuthProvider>
          <Header/>
          <Routes>
            {/* Rotas públicas */}
            <Route path='/login' element={<Login />} />
            <Route path='/teste'element={<Teste/>} />
            
            {/* Rotas protegidas dentro da PrivateRoute*/}
            <Route element={<PrivateRoute />}>
              <Route path='/home' element={<Home />} />
            </Route>

          </Routes>
        </AuthProvider>
      </Router>
  );
}

export default App;
