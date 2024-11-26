import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';

import LoggedRoute from './components/serviceComponents/LoggedRoute';
import AuthenticatedRoute from './components/serviceComponents/AuthenticatedRoute';
import { AuthProvider } from './services/AuthContext';

import Header from './components/molecules/Header';
import PageWrapper from './components/serviceComponents/PageWrapper';

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

            {/* Rotas protegidas por isLogged */}
            <Route element={<LoggedRoute />}>
              <Route path='/vinculos' element={<ListaVinculos />} />
            </Route>

            {/* Rotas protegidas por isAuthenticated */}
            <Route element={<AuthenticatedRoute />}>
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
