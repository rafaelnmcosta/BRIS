import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import TemplateEdicaoUsuario from '../components/templates/TemplateEdicaoUsuario';
import { usuarios } from '../api/usuariosAPI';

const EdicaoUsuario = () => {
    const navigate = useNavigate();
    const { id } = useParams();
    const [usuario, setUsuario] = useState(null);
    const [vinculos, setVinculos] = useState([]);

    // Carrega os dados do usuário ao montar o componente
    useEffect(() => {
        const carregarUsuario = async () => {
            try {
                const dadosUsuario = await usuarios.detalhesUsuario(id);
                setUsuario(dadosUsuario);
                setVinculos(dadosUsuario.vinculos);
            } catch (error) {
                console.error('Erro ao carregar usuário:', error);
                navigate('/usuarios', { replace: true });
            }
        };

        carregarUsuario();
    }, [id, navigate]);

    const handleSubmit = async (formData) => {
        try {
            await usuarios.editarUsuario({
                id: usuario.id,
                nome: formData.nome,
                email: formData.email,
                cpf: usuario.cpf,
                telefone: formData.telefone,
                senha: formData.senha || undefined, // Envia undefined se não mudar
                vinculos: vinculos.map(v => ({
                    roleId: v.roleId,
                    granjaId: v.granjaId || null,
                    agroindustriaId: v.agroindustriaId || null
                }))
            });
            navigate('/usuarios');
        } catch (error) {
            console.error('Erro na atualização:', error);
        }
    };

    const handleAdicionarVinculo = (novoVinculo) => {
        setVinculos([...vinculos, novoVinculo]);
    };

    const handleRemoverVinculo = (id) => {
        setVinculos(prev => prev.filter(vinculo => vinculo.id !== id));
    };

    if (!usuario) {
        return <div className="container mx-auto pt-8">Carregando...</div>;
    }

    return (
        <TemplateEdicaoUsuario
            onSubmit={handleSubmit}
            initialData={usuario}
            vinculos={vinculos}
            onAdicionarVinculo={handleAdicionarVinculo}
            onRemoverVinculo={handleRemoverVinculo}
        />
    );
};

export default EdicaoUsuario;