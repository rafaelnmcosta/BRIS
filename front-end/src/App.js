import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';

import PrivateRoute from './components/PrivateRoute';
import { AuthProvider } from './services/AuthContext';

import Header from './components/molecules/Header';
import PageWrapper from './components/PageWrapper';

import Login from './pages/Login';
import AutoCadastro from './pages/AutoCadastro';
import ListaVinculos from './pages/ListaVinculos';
import Home from './pages/Home';
import Teste from './pages/Teste';

function App() {
  return (
    <Router>
      <AuthProvider>
        <Header />
        <PageWrapper>
          <Routes>
            {/* Rotas públicas */}
            <Route path='/login' element={<Login />} />
            <Route path='/cadastro' element={<AutoCadastro />} />
            <Route path='/vinculos' element={<ListaVinculos />} />

            {/* Rotas protegidas dentro da PrivateRoute*/}
            <Route element={<PrivateRoute />}>
              <Route path='/home' element={<Home />} />
              <Route path='/teste' element={<Teste />} />
            </Route>

          </Routes>
        </PageWrapper>
      </AuthProvider>
    </Router>
  );
}

export default App;
