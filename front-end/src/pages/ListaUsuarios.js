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

  useEffect(() => {
    const fetchUsuarios = async () => {
      try {
        const response = await api.get('http://localhost:5206/api/Usuarios/usuarios');
        setUsuarios(response.data);
      } catch (error) {
        console.error('Erro ao buscar os Usuarios:', error);
      }
    };

    fetchUsuarios();
  }, []);

  const handleEdit = (usuarioId) => {
    // Lógica para editar os dados do usuario com o ID fornecido
    console.log('Editar dados do usuário com ID:', usuarioId);
  };

  return (
    <div>
        <HeadbarSistema />
        <div className='page-content'>
            <a href='/'> {'< '} Voltar</a>
            <div className='lado-a-lado'>
              <h2 className='title'>Lista de usuários</h2>
              <div className='button-bar'>
                <button onClick={() => navigate('/usuarios/cadastrar')}className='button-com-icone'>
                  <IconCadastro className='icone-botao'/>
                  Cadastrar novo usuário
                </button>
                <button onClick={() => navigate('/usuarios/ativar')} className='button-com-icone'>
                  <IconGerenciar className='icone-botao'/>
                  Ativar usuários
                </button>
              </div>
            </div>
            {usuarios.map(usuario => (
                <div key={usuario.id} style={{ marginBottom: 16 }}>
                    <CardUsuario
                    id={usuario.id}
                    nome={usuario.nome}
                    email={usuario.email}
                    tipo={usuario.tipo}
                    onEdit={() => handleEdit(usuario.id)}
                    />
                </div>
            ))}
        </div>
    </div>
  );
};

export default ListaUsuarios;
