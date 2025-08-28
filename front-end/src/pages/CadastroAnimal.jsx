import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import TemplateCadastroAnimal from '../components/templates/TemplateCadastroAnimal';
import { animais } from '../api/animaisAPI';
import { agroindustrias } from '../api/agroindustriasAPI';
import { granjas } from '../api/granjasAPI';
import { usuarios } from '../api/usuariosAPI';

const CadastroAnimal = () => {
    const navigate = useNavigate();
    const [erros, setErros] = useState({});
    const [error, setError] = useState(null);

    // === Funções de carregamento ===
    const carregarAgroindustrias = async () => {
        try {
            const dados = await agroindustrias.listarAgroindustrias();
            return dados;
        } catch (err) {
            setError('Falha ao carregar lista de agroindústrias');
            return [];
        }
    };

    const carregarGranjas = async (agroId) => {
        try {
            let dados = [];
            if (agroId) {
                dados = await granjas.listarPorAgroindustria(agroId);
            }
            return dados;
        } catch (err) {
            setError('Falha ao carregar lista de granjas');
            return [];
        }
    };

    const carregarUsuarios = async () => {
        try {
            const dados = await usuarios.listarUsuarios();
            return dados;
        } catch (err) {
            setError('Falha ao carregar lista de usuários');
            return [];
        }
    };

    // === Submit do formulário ===
    const handleSubmit = async (formData) => {
        const novosErros = {};

        if (!formData.linhagem) novosErros.linhagem = 'Linhagem é obrigatória';
        if (!formData.idade || formData.idade <= 0) novosErros.idade = 'Idade deve ser positiva';
        if (!formData.peso || formData.peso <= 0) novosErros.peso = 'Peso deve ser positivo';
        if (!formData.granjaId || formData.granjaId <= 0) novosErros.granjaId = 'Granja é obrigatória';
        if (!formData.usuarioResponsavelId || formData.usuarioResponsavelId <= 0) novosErros.usuarioResponsavelId = 'Responsável é obrigatório';
        if (typeof formData.ativo !== 'boolean') novosErros.ativo = 'Estado de ativação é obrigatório';

        if (Object.keys(novosErros).length > 0) {
            setErros(novosErros);
            return;
        }

        try {
            await animais.cadastrarAnimal(formData);
            navigate('/animais');
        } catch (err) {
            console.error('Erro ao cadastrar animal:', err);
        }
    };

    if (error) return <p className="text-red-500">{error}</p>;

    return (
        <TemplateCadastroAnimal
            onSubmit={handleSubmit}
            erros={erros}
            carregarAgroindustrias={carregarAgroindustrias}
            carregarGranjas={carregarGranjas}
            carregarUsuarios={carregarUsuarios}
        />
    );
};

export default CadastroAnimal;
