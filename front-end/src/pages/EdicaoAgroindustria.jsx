import React, { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import TemplateEdicaoAgroindustria from '../components/templates/TemplateEdicaoAgroindustria';
import { agroindustrias } from '../api/agroindustriasAPI';

const EdicaoAgroindustria = () => {
    const { id } = useParams();
    const navigate = useNavigate();
    const [erros, setErros] = useState({});
    const [agroindustria, setAgroindustria] = useState(null);

    useEffect(() => {
        const carregarAgroindustria = async () => {
            try {
                const dados = await agroindustrias.detalhesAgroindustria(id);
                setAgroindustria(dados);
            } catch (error) {
                console.error('Erro ao carregar agroindústria:', error);
                navigate('/agroindustrias', { replace: true });
            }
        };

        carregarAgroindustria();
    }, [id, navigate]);

    const handleSubmit = async (formData) => {
        const novosErros = {};
        if (!formData.razaoSocial) novosErros.razaoSocial = 'Razão Social é obrigatória';
        if (!formData.nomeFantasia) novosErros.nomeFantasia = 'Nome Fantasia é obrigatória';
        if (!formData.cnpj) novosErros.cnpj = 'CNPJ é obrigatório';

        if (Object.keys(novosErros).length > 0) {
            setErros(novosErros);
            return;
        }

        try {
            await agroindustrias.editarAgroindustria(agroindustria.id, {
                razaoSocial: formData.razaoSocial,
                nomeFantasia: formData.nomeFantasia,
                cnpj: formData.cnpj
            });
            navigate('/agroindustrias');
        } catch (error) {
            console.error('Erro ao editar agroindústria:', error);
            setErros(error.response?.data || { message: 'Erro ao atualizar agroindústria' });
        }
    };

    if (!agroindustria) {
        return <div className="container mx-auto pt-8">Carregando...</div>;
    }

    return (
        <TemplateEdicaoAgroindustria
            onSubmit={handleSubmit}
            erros={erros}
            initialData={agroindustria}
        />
    );
};

export default EdicaoAgroindustria;
