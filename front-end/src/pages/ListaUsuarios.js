import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import CardUsuario from '../components/CardUsuario';
import HeadbarSistema from '../components/HeadbarSistema';
import api from '../services/api';

import './Listas.css';

import { ReactComponent as IconCadastro } from '../assets/icones/plus-add-cross-outline-svgrepo-com.svg';
import { ReactComponent as IconGerenciar } from '../assets/icones/users-button-outline-svgrepo-com.svg';

const ListaUsuarios = () => {
  const [usuarios, setUsuarios] = useState([]);
  const navigate = useNavigate();

  const tipoUsuario = localStorage.getItem('tipoUsuario');
  // const tipoUsuario = '1';

  useEffect(() => {
    /*
    const fetchUsuarios = async () => {
      try {
        const response = await api.get('http://localhost:5206/api/Usuarios/usuarios');
        setUsuarios(response.data);
      } catch (error) {
        console.error('Erro ao buscar os Usuarios:', error);
      }
    };

    fetchUsuarios();
    */
    const usuariosTeste = [
      { id: 1, nome: 'João Silva', email: 'joao.silva@example.com', tipo: 'Admin' },
      { id: 2, nome: 'Maria Oliveira', email: 'maria.oliveira@example.com', tipo: 'Gerente' },
      { id: 3, nome: 'Pedro Santos', email: 'pedro.santos@example.com', tipo: 'Técnico' },
      { id: 4, nome: 'Ana Costa', email: 'ana.costa@example.com', tipo: 'Admin' }
    ];
    setUsuarios(usuariosTeste);
  }, []);

  const handleEdit = (usuarioId) => {
    console.log('Editar dados do usuário com ID:', usuarioId);
    // Lógica de edição
  };

  return (
    <div>
      <HeadbarSistema />
      <div className='page-content'>
        <a href='/'> {'< '} Voltar</a>
        <div className='lado-a-lado'>
          <h2 className='title'>Lista de usuários</h2>
          {tipoUsuario === '1' || tipoUsuario === '2' || tipoUsuario === '3' ? (
            <div className='button-bar'>
              <button onClick={() => navigate('/usuarios/cadastrar')} className='button-com-icone'>
                <IconCadastro className='icone-botao' />
                Cadastrar novo usuário
              </button>
              <button onClick={() => navigate('/usuarios/ativar')} className='button-com-icone'>
                <IconGerenciar className='icone-botao' />
                Ativar usuários
              </button>
            </div>
          ) : null}
        </div>
        {usuarios.map(usuario => (
          <div key={usuario.id} style={{ marginBottom: 16 }}>
            <CardUsuario
              id={usuario.id}
              nome={usuario.nome}
              email={usuario.email}
              tipo={usuario.tipo}
              onEdit={tipoUsuario === '1' || tipoUsuario === '2' || tipoUsuario === '3' ? () => handleEdit(usuario.id) : null}
            />
          </div>
        ))}
      </div>
    </div>
  );
};

export default ListaUsuarios;
