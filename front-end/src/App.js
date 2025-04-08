import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';

// imports de componentes de funcionamento
import LoggedRoute from './components/serviceComponents/LoggedRoute';
import AuthenticatedRoute from './components/serviceComponents/AuthenticatedRoute';
import { AuthProvider } from './services/AuthContext';
import { NotificationProvider } from './services/NotificationContext';

// header e outros comuns a todas as paginas
import Header from './components/organisms/Header';

// imports de paginas
import Login from './pages/Login';
import ListaVinculos from './pages/ListaVinculos';
import Home from './pages/Home';
import Teste from './pages/Teste';
import TabelaUsuarios from './pages/TabelaUsuarios';
import CadastroUsuario from './pages/CadastroUsuario';

function App() {
  return (
    <Router>
      <NotificationProvider>
        <AuthProvider>
          <Header />
          <Routes>
            {/* Rotas públicas */}
            <Route path='/login' element={<Login />} />
            <Route path='/teste' element={<CadastroUsuario />} />

            {/* Rotas protegidas por isLogged */}
            <Route element={<LoggedRoute />}>
              <Route path='/vinculos' element={<ListaVinculos />} />
            </Route>

            {/* Rotas protegidas por isAuthenticated */}
            <Route element={<AuthenticatedRoute />}>
              <Route path='/home' element={<Home />} />
              <Route path='/teste' element={<Teste />} />
              <Route path='/usuarios' element={<TabelaUsuarios />} />
              <Route path="/usuarios/cadastrar" element={<CadastroUsuario />} />
            </Route>
          </Routes>
        </AuthProvider>
      </NotificationProvider>
    </Router>
  );
}


export default App;
