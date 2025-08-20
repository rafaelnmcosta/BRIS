import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import TemplateCadastroGranja from '../components/templates/TemplateCadastroGranja';
import { granjas } from '../api/granjasAPI';
import { agroindustrias } from '../api/agroindustriasAPI';
import { useAuth } from '../services/AuthContext';

const CadastroGranja = () => {
    const navigate = useNavigate();
    const { isAuthenticated } = useAuth();

    const [erros, setErros] = useState({});
    const [agroLista, setAgroLista] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    // carrega agroindústrias
    const carregarAgroindustrias = async () => {
        try {
            const dados = await agroindustrias.listarAgroindustrias();
            setAgroLista(dados);
        } catch (err) {
            setError('Falha ao carregar lista de agroindústrias');
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        if (isAuthenticated) {
            carregarAgroindustrias();
        }
    }, [isAuthenticated]);

    const handleSubmit = async (formData) => {
        const novosErros = {};

        if (!formData.nomePropriedade) novosErros.nomePropriedade = 'Nome da granja é obrigatório';
        if (!formData.endereco) novosErros.endereco = 'Endereço é obrigatório';
        if (!formData.agroindustriaId || formData.agroindustriaId <= 0) novosErros.agroindustriaId = 'Agroindústria é obrigatória';
        if (!formData.cnpj || formData.cnpj.length !== 14) novosErros.cnpj = 'CNPJ deve ter 14 dígitos';

        if (Object.keys(novosErros).length > 0) {
            setErros(novosErros);
            return;
        }

        try {
            await granjas.cadastrarGranja({
                nomePropriedade: formData.nomePropriedade,
                endereco: formData.endereco,
                agroindustriaId: formData.agroindustriaId,
                cnpj: formData.cnpj,
                email: formData.email || '',
                telefone: formData.telefone || ''
            });
            navigate('/granjas');
        } catch (error) {
            console.error('Erro ao cadastrar granja:', error);
        }
    };

    if (loading) return <p>Carregando agroindústrias...</p>;
    if (error) return <p className="text-red-500">{error}</p>;

    return (
        <TemplateCadastroGranja
            onSubmit={handleSubmit}
            erros={erros}
            agroLista={agroLista}
        />
    );
};

export default CadastroGranja;
