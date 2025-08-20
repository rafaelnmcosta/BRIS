import React, { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import TemplateEdicaoGranja from '../components/templates/TemplateEdicaoGranja';
import { granjas } from '../api/granjasAPI';
import { message } from 'antd';

const EdicaoGranja = () => {
    const { id } = useParams();
    const navigate = useNavigate();
    const [erros, setErros] = useState({});
    const [granja, setGranja] = useState(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const carregarGranja = async () => {
            try {
                const dados = await granjas.detalhesGranja(id);
                setGranja(dados);
            } catch (error) {
                console.error('Erro ao carregar granja:', error);
                message.error('Não foi possível carregar a granja.');
                navigate('/granjas', { replace: true });
            } finally {
                setLoading(false);
            }
        };

        carregarGranja();
    }, [id, navigate]);

    const handleSubmit = async (formData) => {
        const novosErros = {};
        if (!formData.nomePropriedade) novosErros.nomePropriedade = 'Nome da granja é obrigatório';
        if (!formData.cnpj) novosErros.cnpj = 'CNPJ é obrigatório';

        if (Object.keys(novosErros).length > 0) {
            setErros(novosErros);
            return;
        }

        try {
            await granjas.editarGranja(granja.id, {
                nomePropriedade: formData.nomePropriedade,
                cnpj: formData.cnpj,
                telefone: formData.telefone,
                email: formData.email,
                agroindustriaId: granja.agroindustria.id
            });

            message.success('Granja atualizada com sucesso!');
            navigate('/granjas');
        } catch (error) {
            console.error('Erro ao editar granja:', error);
            setErros({ form: error?.message || 'Erro ao atualizar granja' });
            message.error(error?.message || 'Erro ao atualizar granja');
        }
    };

    if (loading || !granja) {
        return <div className="container mx-auto pt-8">Carregando...</div>;
    }

    return (
        <TemplateEdicaoGranja
            onSubmit={handleSubmit}
            erros={erros}
            initialData={granja}
        />
    );
};

export default EdicaoGranja;
